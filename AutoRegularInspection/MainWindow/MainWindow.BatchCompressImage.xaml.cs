using AutoRegularInspection.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Threading;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void BatchCompressImage_Click(object sender, RoutedEventArgs e)
        {
            var serializer = new XmlSerializer(typeof(OptionConfiguration));
            StreamReader reader = new StreamReader($"{App.ConfigurationFolder}\\{App.ConfigFileName}");    //TODO：找不到文件的判断
            var deserializedConfig = (OptionConfiguration)serializer.Deserialize(reader);
            double CompressImageWidth = deserializedConfig.Picture.CompressWidth;
            double CompressImageHeight = deserializedConfig.Picture.CompressHeight;

            // 创建新的 ProgressBarWindow 和 ProgressBarModel
            ProgressBarWindow w = new ProgressBarWindow();
            ProgressBarModel progressBarModel = new ProgressBarModel
            {
                ProgressValue = 0
            };
            w.DataContext = progressBarModel;  // 设置 DataContext

            // 设置 ProgressBarWindow 的位置
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.4 * (App.ScreenWidth - w.Width);

            w.Show();

            Task.Run(() =>
            {
                try
                {
                    var imageProcessor = new ImageProcessor();
                    imageProcessor.ProcessImages(App.PicturesFolder, App.PicturesOutFolder, CompressImageWidth, CompressImageHeight, new Progress<ProgressReport>(report =>
                    {
                        progressBarModel.ProgressValue = report.ProgressPercentage;
                        progressBarModel.Content = report.CurrentOperation;

                    }), progressBarModel.CancellationTokenSource.Token);

                    w.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("图片压缩完成！"); });
                }
                catch (OperationCanceledException)
                {
                    w.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("图片压缩已取消！"); });
                }
                catch (Exception ex)
                {
                    w.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show(ex.Message.ToString(CultureInfo.InvariantCulture)); });
                }
                finally
                {
                    w.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                }
            });

        }
    }

    public class ImageProcessor
    {
        public List<string> ProcessImages(string sourceDirectory, string outputDirectory, double targetWidth, double targetHeight, IProgress<ProgressReport> progress, CancellationToken cancellationToken)
        {
            if (targetWidth < 0)
            {
                throw new ArgumentException("目标宽度不为负数", nameof(targetWidth));
            }

            if (targetHeight < 0)
            {
                throw new ArgumentException("目标高度不为负数", nameof(targetHeight));
            }

            string[] searchPatterns = new[] { "*.jpg", "*.png", "*.jpeg", "*.bmp" };
            var imageFiles = searchPatterns.SelectMany(pattern => Directory.GetFiles(sourceDirectory, pattern, SearchOption.AllDirectories)).ToArray();

            List<string> outputFiles = new List<string>();
            int counter = 0;
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount / 2,
                CancellationToken = cancellationToken,
            };

            Parallel.ForEach(imageFiles, options, filename =>
            {
                cancellationToken.ThrowIfCancellationRequested(); // 检查是否取消

                string name = Path.GetFileName(filename);
                using (Image image = Image.Load<Rgba32>(filename))
                {
                    int width = (int)targetWidth;
                    int height = (int)targetHeight;
                    image.Mutate(x => x.Resize(width, height, KnownResamplers.Bicubic));
                    string outputPath = $"{outputDirectory}\\{name}";
                    image.Save(outputPath);
                    outputFiles.Add(outputPath);

                    int incrementedCounter = Interlocked.Increment(ref counter);
                    progress.Report(new ProgressReport
                    {
                        ProgressPercentage = incrementedCounter * 100 / imageFiles.Length,
                        CurrentOperation = $"已处理图片{incrementedCounter}/{imageFiles.Length}"
                    });
                }
            });

            return outputFiles;
        }
    }

    public class ProgressReport
    {
        public int ProgressPercentage { get; set; }
        public string CurrentOperation { get; set; }
    }

}
