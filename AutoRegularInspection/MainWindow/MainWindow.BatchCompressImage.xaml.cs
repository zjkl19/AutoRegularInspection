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

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void BatchCompressImage_Click(object sender, RoutedEventArgs e)
        {
            System.Configuration.Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            XDocument config = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");
            XElement compressPictureWidth = config.Elements("configuration").Elements("Picture").Elements("CompressWidth").FirstOrDefault();
            XElement compressPictureHeight = config.Elements("configuration").Elements("Picture").Elements("CompressHeight").FirstOrDefault();
            double CompressImageWidth = Convert.ToDouble(compressPictureWidth.Value, CultureInfo.InvariantCulture);
            double CompressImageHeight = Convert.ToDouble(compressPictureHeight.Value, CultureInfo.InvariantCulture);

            //参考：https://docs.sixlabors.com/articles/imagesharp/resize.html
            try
            {
                ProgressBarWindow w = new ProgressBarWindow();
                w.Top = 0.4 * (App.ScreenHeight - w.Height);
                w.Left = 0.4 * (App.ScreenWidth - w.Width);

                ProgressBarModel progressBarModel = new ProgressBarModel
                {
                    ProgressValue = 0
                };
                w.progressBarNumberTextBlock.DataContext = progressBarModel;
                w.progressBar.DataContext = progressBarModel;
                w.progressBarContentTextBlock.DataContext = progressBarModel;

                int progressSleepTime = 500;    //进度条停顿时间

                Thread thread = new Thread(new ThreadStart(() =>
                {

                    w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Show(); });
                    var jpgFiles = Directory.GetFiles($"{App.PicturesFolder}", "*.jpg", SearchOption.AllDirectories);
                    int counter = 0;
                    foreach (string filename in jpgFiles)
                    {
                        string name = Path.GetFileName(filename);
                        using (Image image = Image.Load<Rgba32>(filename))
                        {

                            int width = (int)CompressImageWidth;
                            int height = (int)CompressImageHeight;
                            image.Mutate(x => x.Resize(width, height, KnownResamplers.Bicubic));
                            image.Save($"{App.PicturesOutFolder}\\{name}");
                            counter++;
                            progressBarModel.ProgressValue = counter * 100 / jpgFiles.Length;
                            progressBarModel.Content = $"已处理图片{counter}/{jpgFiles.Length}";
                        }
                    }
                    w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                    w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("图片压缩完成！"); });

                }));
                thread.Start();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(CultureInfo.InvariantCulture));
            }



        }
    }
}
