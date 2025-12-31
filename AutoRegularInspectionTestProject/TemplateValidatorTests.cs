using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.IO;
using Xunit;

namespace AutoRegularInspectionTestProject
{
    public class TemplateValidatorTests
    {
        [Fact]
        public void Validate_ReturnsFalse_WhenFileMissing()
        {
            var template = new ComboBoxReportTemplates
            {
                Name = "自定义外观检查报告模板.docx",
                DisplayName = "自定义报告模板"
            };

            string message;
            var result = TemplateValidator.Validate(template, Path.GetTempPath(), out message);

            Assert.False(result);
            Assert.False(string.IsNullOrWhiteSpace(message));
        }

        [Fact]
        public void Validate_ReturnsTrue_WhenFileExistsAndStrategyAvailable()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var filePath = Path.Combine(tempDir, "自定义外观检查报告模板.docx");
            File.WriteAllText(filePath, "placeholder");

            var template = new ComboBoxReportTemplates
            {
                Name = "自定义外观检查报告模板.docx",
                DisplayName = "自定义报告模板"
            };

            string message;
            var result = TemplateValidator.Validate(template, tempDir, out message);

            Assert.True(result);
            Assert.True(string.IsNullOrWhiteSpace(message));
        }
    }
}
