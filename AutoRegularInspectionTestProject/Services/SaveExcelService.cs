using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace AutoRegularInspectionTestProject.Services
{
    public class SaveExcelServie
    {
        [Fact]
        public void SaveExcel()
        {
            //Arrange
            var listDamageSummary = new List<DamageSummary>();
            listDamageSummary.Add(new DamageSummary { Position = "第一跨" });
            //Act
            SaveExcelService.SaveExcel(listDamageSummary);

            var file = new FileInfo("temp外观检查.xlsx");

            string testString = string.Empty;

            using (var excelPackage = new ExcelPackage(file))
            {
                // 添加worksheet
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                testString = worksheet.Cells[2, 2].Value?.ToString() ?? string.Empty;
            }

            //Assert
            Assert.Equal("第2跨", testString);
        }
    }
}
