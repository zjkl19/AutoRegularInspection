using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Linq;

namespace AutoRegularInspectionTestProject.Services
{
    public class SaveExcelServie
    {
        [Fact]
        public void SaveExcel_ShouldSaveVarInExcel()
        {
            //GlobalData.ComponentComboBox[bridgeDeckListDamageSummary[i].ComponentValue].DamageComboBox[bridgeDeckListDamageSummary[i].DamageValue].Title
            //Arrange
            string saveFileName = "外观检查_ShouldSaveVarInExcel.xlsx";
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "第1跨"
                    , ComponentValue=GlobalData.ComponentComboBox.Where(x=>x.Title=="伸缩缝").FirstOrDefault().Idx
                    , Component = string.Empty
                    ,DamageValue=GlobalData.ComponentComboBox.Where(x=>x.Title=="伸缩缝").FirstOrDefault().DamageComboBox.Where(p=>p.Title=="缝内沉积物阻塞").FirstOrDefault().Idx    //若节约时间不写复杂表达式，直接填1（仅对本次测试有效）
                    ,Damage="缝内沉积物阻塞"
                    ,DamageDescription="左幅0#伸缩缝沉积物阻塞"
                    ,Comment="新增"
                }
            };
            var superSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "第1跨"
                    , ComponentValue=GlobalData.SuperSpaceComponentComboBox.Where(x=>x.Title=="主梁").FirstOrDefault().Idx
                    , Component = string.Empty
                    ,DamageValue=GlobalData.SuperSpaceComponentComboBox.Where(x=>x.Title=="主梁").FirstOrDefault().DamageComboBox.Where(p=>p.Title=="其它").FirstOrDefault().Idx
                    ,Damage="无"
                    ,DamageDescription="无"
                }
            };
            var subSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "0#台"
                    , ComponentValue=GlobalData.SubSpaceComponentComboBox.Where(x=>x.Title=="台身").FirstOrDefault().Idx
                    , Component = "台身"
                    ,DamageValue=GlobalData.SubSpaceComponentComboBox.Where(x=>x.Title=="台身").FirstOrDefault().DamageComboBox.Where(p=>p.Title=="其它").FirstOrDefault().Idx
                    ,Damage="水蚀"
                    ,DamageDescription="右幅0#台台身水蚀"
                }
            };

            //默认测试桥面系标签
            string expectedDamage = bridgeDeckListDamageSummary[0].Damage; string acturalDamage = string.Empty;
            string expectedDamageDescription = bridgeDeckListDamageSummary[0].DamageDescription; string acturalDamageDescription = string.Empty;
            string expectedComment = bridgeDeckListDamageSummary[0].Comment; string acturalComment = string.Empty;

            string expectedDamageInSuperStructure = "无"; string acturalDamageInSuperStructure = string.Empty;
            string expectedDamageInSubStructure = "水蚀"; string acturalDamageInSubStructure = string.Empty;
            //Act

            SaveExcelService.SaveExcel(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary, saveFileName);

            var file = new FileInfo(saveFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                acturalDamage = worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "缺损类型")].Value?.ToString() ?? string.Empty;
                acturalDamageDescription = worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "缺损描述")].Value?.ToString() ?? string.Empty;
                acturalComment = worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "备注")].Value?.ToString() ?? string.Empty;

                // 检查"上部结构"Worksheets
                worksheet = excelPackage.Workbook.Worksheets["上部结构"];
                acturalDamageInSuperStructure = worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "缺损类型")].Value?.ToString() ?? string.Empty;

                worksheet = excelPackage.Workbook.Worksheets["下部结构"];
                acturalDamageInSubStructure = worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "缺损类型")].Value?.ToString() ?? string.Empty;
            }

            //Assert
            //桥面系
            Assert.Equal(expectedDamage, acturalDamage);
            Assert.Equal(expectedDamageDescription, acturalDamageDescription);
            Assert.Equal(expectedComment, acturalComment);
            //上部结构
            Assert.Equal(expectedDamageInSuperStructure, acturalDamageInSuperStructure);
            //下部结构
            Assert.Equal(expectedDamageInSubStructure, acturalDamageInSubStructure);
            //Assert.Equal(0, 1);    //TODO：初始化变量重构
        }

        [Fact]
        public void SaveExcel_ShouldSaveCorrectComponentInExcel_WhileComponentIsNotOthers()
        {
            //Arrange
            string saveFileName = "外观检查_ShouldSaveCorrectComponentInExcel.xlsx";
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "第1跨"
                    ,ComponentValue=2    //"伸缩缝"
                    , Component = string.Empty
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
            string expectedComponent = "伸缩缝"; string acturalComponent = string.Empty;
            //Act

            SaveExcelService.SaveExcel(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary, saveFileName);

            var file = new FileInfo(saveFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                acturalComponent = worksheet.Cells[2, 3].Value?.ToString() ?? string.Empty;
                // 检查"上部结构"Worksheets
                worksheet = excelPackage.Workbook.Worksheets["上部结构"];

                worksheet = excelPackage.Workbook.Worksheets["下部结构"];
            }
            if (File.Exists(saveFileName))
            {
                File.Delete(saveFileName);
            }
            //Assert
            //桥面系
            Assert.Equal(expectedComponent, acturalComponent);
        }

        [Fact]
        public void SaveExcel_ShouldSaveCorrectComponentInExcel_WhileComponentIsOthers()
        {
            //Arrange
            string saveFileName = "外观检查_ShouldSaveCorrectComponentInExcel.xlsx";
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "第1跨"
                    ,ComponentValue=GlobalData.ComponentComboBox.Where(x=>x.Title=="其它").FirstOrDefault().Idx   //"其它"
                    , Component = "其它部件"    //"其它部件"是自己随便想的名称
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
            string expectedComponent = "其它部件"; string acturalComponent = string.Empty;
            //Act

            SaveExcelService.SaveExcel(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary, saveFileName);

            var file = new FileInfo(saveFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 只测试"桥面系"示意
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                acturalComponent = worksheet.Cells[2, 3].Value?.ToString() ?? string.Empty;
            }
            if (File.Exists(saveFileName))
            {
                File.Delete(saveFileName);
            }
            //Assert
            //桥面系
            Assert.Equal(expectedComponent, acturalComponent);
        }

        [Fact]
        public void SaveExcel_ShouldSaveCorrectDamageInExcel_WhileDamageIsNotOthers()
        {
            //Arrange
            string saveFileName = "外观检查_ShouldSaveCorrectDamageInExcel.xlsx";
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    Position = "第1跨"
                    ,ComponentValue=2    //"伸缩缝"
                    , Component = string.Empty
                    ,DamageValue=1
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
            string expectedDamage = "缝内沉积物阻塞"; string acturalDamage = string.Empty;
            //Act

            SaveExcelService.SaveExcel(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary, saveFileName);

            var file = new FileInfo(saveFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                acturalDamage = worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet,"缺损类型")].Value?.ToString() ?? string.Empty;
                // 检查"上部结构"Worksheets
                worksheet = excelPackage.Workbook.Worksheets["上部结构"];

                worksheet = excelPackage.Workbook.Worksheets["下部结构"];
            }


            if (File.Exists(saveFileName))
            {
                File.Delete(saveFileName);
            }
            //Assert
            //桥面系
            Assert.Equal(expectedDamage, acturalDamage);
        }


        [Fact]
        public void FindColumnIndexByName_ShouldReturnCorrectColumnIndex_WhileColumnExists()
        {
            //Arrange
            string fileName = "列索引测试_ShouldReturnCorrectColumnIndex.xlsx";

            //删除干扰文件
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var file = new FileInfo(fileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("桥面系");
                //添加表头
                worksheet.Cells[1, 1].Value = "序号";
                worksheet.Cells[1, 2].Value = "位置";
                worksheet.Cells[1, 3].Value = "要素";
                worksheet.Cells[1, 4].Value = "缺损类型";
                worksheet.Cells[1, 5].Value = "缺损描述";
                worksheet.Cells[1, 6].Value = "图片描述";
                worksheet.Cells[1, 7].Value = "照片编号";
                excelPackage.Save();
            }

            int expectedColumn = 3;
            int searchedColumn = 0;
            //Act

            using (var excelPackage = new ExcelPackage(file))
            {
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                searchedColumn = SaveExcelService.FindColumnIndexByName(worksheet, "要素");
            }

            //删除临时文件
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //Assert
            //桥面系
            Assert.Equal(expectedColumn, searchedColumn);
        }

        [Fact]
        public void FindColumnIndexByName_ShouldReturnZeroColumnIndex_WhileColumnNotExists()
        {
            //Arrange
            string fileName = "列索引测试_ShouldReturnZeroColumnIndex.xlsx";

            //删除干扰文件
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var file = new FileInfo(fileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("桥面系");
                //添加表头
                worksheet.Cells[1, 1].Value = "序号";
                worksheet.Cells[1, 2].Value = "位置";
                worksheet.Cells[1, 3].Value = "要素";
                worksheet.Cells[1, 4].Value = "缺损类型";
                worksheet.Cells[1, 5].Value = "缺损描述";
                worksheet.Cells[1, 6].Value = "图片描述";
                worksheet.Cells[1, 7].Value = "照片编号";
                excelPackage.Save();
            }

            int expectedColumn = 0;
            int searchedColumn = 0;
            //Act

            using (var excelPackage = new ExcelPackage(file))
            {
                var worksheet = excelPackage.Workbook.Worksheets["桥面系"];
                searchedColumn = SaveExcelService.FindColumnIndexByName(worksheet, "要素1");
            }

            //删除临时文件
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //Assert
            //桥面系
            Assert.Equal(expectedColumn, searchedColumn);
        }
    }
}
