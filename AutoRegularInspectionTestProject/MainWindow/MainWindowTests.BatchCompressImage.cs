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

        private readonly string _sourceDirectory;
        private readonly string _emptyDirectory;
        private readonly string _outputDirectory;

        private readonly double _targetWidth = 100;
        private readonly double _targetHeight = 100;

        

        public ImageProcessorTests()
        {
            // 初始化图片文件夹路径
            _inputFolderPath = Path.Combine(Path.GetTempPath(), "ImageProcessorTests", "Input");
            _outputFolderPath = Path.Combine(Path.GetTempPath(), "ImageProcessorTests", "Output");

            _sourceDirectory = "source";
            _emptyDirectory = "emptyDirectory";
            _outputDirectory = "output";

            // 确保文件夹存在
            Directory.CreateDirectory(_inputFolderPath);
            Directory.CreateDirectory(_outputFolderPath);

            Directory.CreateDirectory(_sourceDirectory);
            Directory.CreateDirectory(_emptyDirectory);
            Directory.CreateDirectory(_outputDirectory);

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
                using var image = Image.Load<Rgba32>(outputFile);
                Assert.Equal(_targetWidth, image.Width);
                Assert.Equal(_targetHeight, image.Height);
            }
        }

        private void GenerateTestImages(string folderPath, int count)
        {
            for (var i = 0; i < count; i++)
            {
                using var image = new Image<Rgba32>(SixLabors.ImageSharp.Configuration.Default, 500, 500);
                image.Save(Path.Combine(folderPath, $"test{i}.png"));
            }
        }

        [Fact]
        public void ProcessImages_ShouldThrowExceptionIfSourceDirectoryDoesNotExist()
        {
            var processor = new ImageProcessor();
            Assert.Throws<DirectoryNotFoundException>(() => processor.ProcessImages("nonexistent", "output", 100, 100, null, new CancellationToken()));
        }

        [Fact]
        public void ProcessImages_ShouldReturnEmptyListIfNoImagesInSourceDirectory()
        {
            var processor = new ImageProcessor();
            var result = processor.ProcessImages("emptyDirectory", "output", 100, 100, null, new CancellationToken());
            Assert.Empty(result);
        }

        [Fact]
        public void ProcessImages_ShouldThrowExceptionIfTargetDimensionsAreInvalid()
        {
            var processor = new ImageProcessor();
            //SixLabors.ImageSharp会自动处理宽或高为0的情况，所以不会抛出异常
            //Assert.Throws<ArgumentException>(() => processor.ProcessImages("source", "output", 0, 100, null, new CancellationToken()));
            //Assert.Throws<ArgumentException>(() => processor.ProcessImages("source", "output", 100, 0, null, new CancellationToken()));
            Assert.Throws<ArgumentException>(() => processor.ProcessImages("source", "output", -1, 100, null, new CancellationToken()));
            Assert.Throws<ArgumentException>(() => processor.ProcessImages("source", "output", 100, -1, null, new CancellationToken()));
        }

        public void Dispose()
        {
            // 清理创建的文件和文件夹
            Directory.Delete(_inputFolderPath, true);
            Directory.Delete(_outputFolderPath, true);

            Directory.Delete(_sourceDirectory, true);
            Directory.Delete(_emptyDirectory, true);
            Directory.Delete(_outputDirectory, true);
        }
    }
}
