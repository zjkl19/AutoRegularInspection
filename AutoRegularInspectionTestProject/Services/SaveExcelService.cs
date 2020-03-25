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
        public void SaveExcel_ShouldSaveVarInExcel()
        {
            //Arrange
            var listDamageSummary = new List<DamageSummary>
            {
                new DamageSummary { 
                    Position = "第1跨"
                    , Component = "伸缩缝"
                    ,Damage="缝内沉积物阻塞"
                    ,DamageDescription="左幅0#伸缩缝沉积物阻塞"
                }
            };
            string expectedDamage = listDamageSummary[0].Damage; string acturalDamage = string.Empty;
            string expectedDamageDescription = listDamageSummary[0].DamageDescription; string acturalDamageDescription = string.Empty;
            string tempFileName = "temp外观检查.xlsx";
            //Act

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }

            SaveExcelService.SaveExcel(listDamageSummary);

            var file = new FileInfo(tempFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                acturalDamage = worksheet.Cells[2, 4].Value?.ToString() ?? string.Empty;
                acturalDamageDescription = worksheet.Cells[2, 5].Value?.ToString() ?? string.Empty;
            }
            
            //Assert
            Assert.Equal(expectedDamage, acturalDamage);
            Assert.Equal(expectedDamageDescription, acturalDamageDescription);
        }
    }
}
