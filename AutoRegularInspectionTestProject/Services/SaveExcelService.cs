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
            string tempFileName = "temp外观检查.xlsx";
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary { 
                    Position = "第1跨"
                    , Component = "伸缩缝"
                    ,Damage="缝内沉积物阻塞"
                    ,DamageDescription="左幅0#伸缩缝沉积物阻塞"
                }
            };
            var superSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "第1跨"
                    , Component = "主梁"
                    ,Damage="无"
                    ,DamageDescription="无"
                }
            };
            var subSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "0#台"
                    , Component = "台身"
                    ,Damage="水蚀"
                    ,DamageDescription="右幅0#台台身水蚀"
                }
            };

            //默认测试桥面系标签
            string expectedDamage = bridgeDeckListDamageSummary[0].Damage; string acturalDamage = string.Empty;
            string expectedDamageDescription = bridgeDeckListDamageSummary[0].DamageDescription; string acturalDamageDescription = string.Empty;

            string expectedDamageInSuperStructure = "无"; string acturalDamageInSuperStructure = string.Empty;
            //Act

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }

            SaveExcelService.SaveExcel(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary);

            var file = new FileInfo(tempFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                acturalDamage = worksheet.Cells[2, 4].Value?.ToString() ?? string.Empty;
                acturalDamageDescription = worksheet.Cells[2, 5].Value?.ToString() ?? string.Empty;
                // 检查"上部结构"Worksheets
                worksheet = excelPackage.Workbook.Worksheets["上部结构"];
                acturalDamageInSuperStructure = worksheet.Cells[2, 4].Value?.ToString() ?? string.Empty;
            }

            //Assert
            //桥面系
            Assert.Equal(expectedDamage, acturalDamage);
            Assert.Equal(expectedDamageDescription, acturalDamageDescription);
            //上部结构
            Assert.Equal(expectedDamageInSuperStructure, acturalDamageInSuperStructure);
            //下部结构
            Assert.Equal(expectedDamageInSubStructure, acturalDamageInSubStructure);
        }
    }
}
