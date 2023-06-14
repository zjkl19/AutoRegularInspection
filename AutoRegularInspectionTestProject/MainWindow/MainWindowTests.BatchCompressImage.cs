using Aspose.Words;
using System;
using Xunit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using AutoRegularInspection.IRepository;
using Ninject;
using System.Collections.Generic;
using Aspose.Words.Tables;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using AutoRegularInspection;
using Moq;
using System.Configuration;
using System.Threading;

namespace AutoRegularInspectionTestProject.MainWindow
{
    public class ImageProcessorTests : IDisposable
    {
        private readonly string _inputFolderPath;
        private readonly string _outputFolderPath;
        private readonly double _targetWidth = 100;
        private readonly double _targetHeight = 100;

        public ImageProcessorTests()
        {
            // 初始化图片文件夹路径
            _inputFolderPath = Path.Combine(Path.GetTempPath(), "ImageProcessorTests", "Input");
            _outputFolderPath = Path.Combine(Path.GetTempPath(), "ImageProcessorTests", "Output");

            // 确保文件夹存在
            Directory.CreateDirectory(_inputFolderPath);
            Directory.CreateDirectory(_outputFolderPath);

            // 为测试准备一些图像文件
            GenerateTestImages(_inputFolderPath, 5);
        }

        [Fact]
        public void ProcessImages_ShouldResizeImages()
        {
            var progress = new Mock<IProgress<ProgressReport>>();
            var cancellationToken = new CancellationToken();

            var imageProcessor = new ImageProcessor();
            var outputFiles = imageProcessor.ProcessImages(_inputFolderPath, _outputFolderPath, _targetWidth, _targetHeight, progress.Object, cancellationToken);

            foreach (var outputFile in outputFiles)
            {
                Assert.True(File.Exists(outputFile));
                using (var image = Image.Load<Rgba32>(outputFile))
                {
                    Assert.Equal(_targetWidth, image.Width);
                    Assert.Equal(_targetHeight, image.Height);
                }
            }
        }

        private void GenerateTestImages(string folderPath, int count)
        {
            for (var i = 0; i < count; i++)
            {
                using (var image = new Image<Rgba32>(SixLabors.ImageSharp.Configuration.Default, 500, 500))
                {
                    image.Save(Path.Combine(folderPath, $"test{i}.png"));
                }
            }
        }

        public void Dispose()
        {
            // 清理创建的文件和文件夹
            Directory.Delete(_inputFolderPath, true);
            Directory.Delete(_outputFolderPath, true);
        }
    }
}
