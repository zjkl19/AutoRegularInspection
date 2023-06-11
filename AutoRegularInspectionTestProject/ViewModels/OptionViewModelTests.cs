using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace AutoRegularInspectionTestProject.ViewModels
{
    public class OptionViewModelTests
    {
        [Fact]
        public void SaveFile_ShouldWritesConfigurationToFile()
        {
            // Arrange
            var configuration = new OptionConfiguration();
            var mockFileWriter = new Mock<IFileWriter>();
            var mockSerializer = new Mock<IXmlSerializer<OptionConfiguration>>();

            // setup mockFileWriter to return a valid TextWriter when Create method is called
            var stringWriter = new StringWriter();
            mockFileWriter.Setup(f => f.Create(It.IsAny<string>())).Returns(stringWriter);

            // Act
            OptionViewModel.SaveFile(configuration, mockFileWriter.Object, mockSerializer.Object);

            // Assert
            // Verify that the Serialize method was called once with the correct parameters
            mockSerializer.Verify(s => s.Serialize(It.IsAny<TextWriter>(), configuration), Times.Once());

        }

        [Fact]
        public void SaveFile_ThrowsException_WhenConfigurationIsNull()
        {
            // Arrange
            var mockFileWriter = new Mock<IFileWriter>();
            var mockSerializer = new Mock<IXmlSerializer<OptionConfiguration>>();


            // Assert
            Assert.Throws<ArgumentNullException>(() => OptionViewModel.SaveFile(null, mockFileWriter.Object, mockSerializer.Object));
        }

        [Fact]
        public void SaveFile_ThrowsException_WhenCreateThrowsException()
        {
            // Arrange
            var mockFileWriter = new Mock<IFileWriter>();
            mockFileWriter.Setup(fw => fw.Create(It.IsAny<string>())).Throws(new Exception());

            var mockSerializer = new Mock<IXmlSerializer<OptionConfiguration>>();
            var configuration = new OptionConfiguration();

            // Assert
            Assert.Throws<Exception>(() => OptionViewModel.SaveFile(configuration, mockFileWriter.Object, mockSerializer.Object));
        }

        [Fact]
        public void SaveFile_ThrowsException_WhenCreateReturnsNull()
        {
            // Arrange
            var mockFileWriter = new Mock<IFileWriter>();
            mockFileWriter.Setup(fw => fw.Create(It.IsAny<string>())).Returns((TextWriter)null);

            var mockSerializer = new Mock<IXmlSerializer<OptionConfiguration>>();
            var configuration = new OptionConfiguration();

            // Assert
            Assert.Throws<Exception>(() => OptionViewModel.SaveFile(configuration, mockFileWriter.Object, mockSerializer.Object));
        }
    }
}
