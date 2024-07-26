using AutoRegularInspection;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;
using static AutoRegularInspection.App;

namespace AutoRegularInspectionTestProject
{
    public class TemplateServiceTests
    {
        private const string TestJson = @"
        [
            {
                ""DisplayName"": ""建研-常规定检--晋安区桥梁模板"",
                ""Name"": ""建研-常规定检--晋安区桥梁模板.doc"",
                ""DocStyleOfMainText"": ""晋安正文"",
                ""DocStyleOfTable"": ""晋安表格"",
                ""DocStyleOfPicture"": ""晋安图片""
            },
            {
                ""DisplayName"": ""交通综合评价报告模板"",
                ""Name"": ""交通综合评价报告模板.docx"",
                ""DocStyleOfMainText"": ""迪南交通报告正文"",
                ""DocStyleOfTable"": ""迪南交通报告表格"",
                ""DocStyleOfPicture"": ""迪南交通报告图片""
            }
        ]";

        [Fact]
        public void LoadTemplates_ShouldLoadTemplatesFromJsonFile()
        {
            // Arrange
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
            mockFileSystem.Setup(fs => fs.ReadAllText(It.IsAny<string>())).Returns(TestJson);

            var templateService = new TemplateService(mockFileSystem.Object);

            // Act
            var templates = templateService.LoadTemplates("dummyPath");

            // Assert
            Assert.NotNull(templates);
            Assert.Equal(2, templates.Count);
            Assert.Equal("建研-常规定检--晋安区桥梁模板", templates[0].DisplayName);
            Assert.Equal("交通综合评价报告模板", templates[1].DisplayName);
        }
    }
}
