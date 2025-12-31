using AutoRegularInspection.Services;
using OfficeOpenXml;
using System.IO;
using Xunit;

namespace AutoRegularInspectionTestProject
{
    public class SaveExcelServiceTests
    {
        public SaveExcelServiceTests()
        {
            // EPPlus 4.x 无需 LicenseContext 设置，保持兼容性
        }

        [Fact]
        public void FindColumnIndexByName_ReturnsCorrectIndex()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "A";
                worksheet.Cells[1, 2].Value = "位置";
                worksheet.Cells[1, 3].Value = "缺损类型";

                var idx = SaveExcelService.FindColumnIndexByName(worksheet, "缺损类型");

                Assert.Equal(3, idx);
            }
        }

        [Fact]
        public void FindColumnIndexByName_ReturnsZero_WhenNotFound()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells[1, 1].Value = "A";

                var idx = SaveExcelService.FindColumnIndexByName(worksheet, "不存在");

                Assert.Equal(0, idx);
            }
        }
    }
}
