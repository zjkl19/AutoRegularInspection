using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using AutoRegularInspection.Services;
using System.Collections.ObjectModel;
using Ninject;
using AutoRegularInspection.IRepository;
using System.IO;
using OfficeOpenXml;
using System.Globalization;

namespace AutoRegularInspectionTestProject.Services
{
    public class DamageSummaryServicesTests
    {
        [Fact]
        public void SetPictureCounts_ShouldSetCorretPictureCounts()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875"
                }
            };

            //Act

            DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);

            //Assert
            Assert.Equal(3, bridgeDeckListDamageSummary[0].PictureCounts);
        }

        [Fact]
        public void SetFirstAndLastPictureBookmark_ShouldSetCorretBookmarkName()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875"
                }
            };

           
            //Act

            DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);

            //Assert
            Assert.Equal(1000000, bridgeDeckListDamageSummary[0].FirstPictureBookmarkIndex);
            Assert.Equal($"_Ref1000000", bridgeDeckListDamageSummary[0].FirstPictureBookmark);
            Assert.Equal($"_Ref1000002", bridgeDeckListDamageSummary[0].LastPictureBookmark);
        }

        [Fact]
        public void SetComboBox_ShouldSetCorretComponentValue()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="伸缩缝"
                    ,Damage="缝内沉积物阻塞"
                    ,PictureNo="855,858,875"
                }
            };

            //Act

            DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);
            //Assert
            Assert.Equal(2, bridgeDeckListDamageSummary[0].ComponentValue);
        }

        [Fact]
        public void SetComboBox_ShouldSetCorretComponentValue_WhileBridgePartIsSuperSpace()
        {
            //Arrange
            var listDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="横向联系"
                    ,Damage="连接件断裂"
                    ,PictureNo="855,858,875"
                }
                ,new DamageSummary {

                    Component="横向联系"
                    ,Damage="横隔板网裂"
                    ,PictureNo="900"
                }
            };

            //Act

            DamageSummaryServices.InitListDamageSummary(listDamageSummary,2_000_000,BridgePart.SuperSpace);
            //Assert
            Assert.Equal(1, listDamageSummary[0].ComponentValue);
        }

        [Fact]
        public void SetComboBox_ShouldSetCorretComponentValue_WhileBridgePartIsSubSpace()
        {
            //Arrange
            var listDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="台身"
                    ,Damage="墩身水平裂缝"
                    ,PictureNo="855,858,875"
                }
            };

            //Act

            DamageSummaryServices.InitListDamageSummary(listDamageSummary, 3_000_000, BridgePart.SubSpace);
            //Assert
            Assert.Equal(3, listDamageSummary[0].ComponentValue);
        }

        /// <summary>
        /// WhileDamageFound指的是在非“其它”的“枚举”中找到
        /// </summary>
        [Fact]
        public void SetComboBox_ShouldSetCorrectDamageAndDamageValue_WhileDamageFound()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="伸缩缝"
                    ,Damage="缝内沉积物阻塞"
                    ,PictureNo="855,858,875"
                }
            };

            //Act

            AutoRegularInspection.Services.DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);
            //Assert
            Assert.Equal("螺帽松动", bridgeDeckListDamageSummary[0].DamageComboBox[0].Title);
            Assert.Equal("缝内沉积物阻塞", bridgeDeckListDamageSummary[0].DamageComboBox[1].Title);
            Assert.Equal(1, bridgeDeckListDamageSummary[0].DamageValue);
        }

        [Fact]
        public void SetComboBox_ShouldSetCorrectDamageAndDamageValue_WhileDamageNotFound()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="伸缩缝"
                    ,Damage="其它病害"
                    ,PictureNo="855,858,875"
                }
            };

            //Act
            DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);

            //Assert
            Assert.Equal("螺帽松动", bridgeDeckListDamageSummary[0].DamageComboBox[0].Title);
            Assert.Equal("缝内沉积物阻塞", bridgeDeckListDamageSummary[0].DamageComboBox[1].Title);
            Assert.Equal(10, bridgeDeckListDamageSummary[0].DamageValue);
        }

        [Fact]
        public void GenerateDamageStatisticsTableShouldSetCorrectStatisticsData()
        {
            //Arrange
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            string saveFileName = "桥梁检测病害统计汇总表.xlsx";
            List<DamageSummary> lst1, lst2, lst3;
            
            lst1 = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            lst2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            lst3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(lst1);
            DamageSummaryServices.InitListDamageSummary(lst2, 2_000_000, BridgePart.SuperSpace);
            DamageSummaryServices.InitListDamageSummary(lst3, 3_000_000, BridgePart.SubSpace);

            ObservableCollection<DamageSummary> oc1 = new ObservableCollection<DamageSummary>();
            ObservableCollection<DamageSummary> oc2 = new ObservableCollection<DamageSummary>();
            ObservableCollection<DamageSummary> oc3 = new ObservableCollection<DamageSummary>();

            lst1.ForEach(x => oc1.Add(x)); lst2.ForEach(x => oc2.Add(x)); lst3.ForEach(x => oc3.Add(x));

            int expectedUnit1TotalCounts = 3; int acturalUnit1TotalCounts = 0;
            decimal expectedUnit2TotalCounts = 29.8m; decimal acturalUnit2TotalCounts = 0.0m;

            //Act
            DamageSummaryServices.GenerateDamageStatisticsTable(oc1,oc2,oc3);
            var file = new FileInfo(saveFileName);
            using (var excelPackage = new ExcelPackage(file))
            {
                // 检查"桥面系"Worksheets
                var worksheet = excelPackage.Workbook.Worksheets["桥面系病害统计汇总表"];
                acturalUnit1TotalCounts = Convert.ToInt32(worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "单位1数量")].Value?.ToString() ?? string.Empty, CultureInfo.InvariantCulture);
                acturalUnit2TotalCounts = Convert.ToDecimal(worksheet.Cells[2, SaveExcelService.FindColumnIndexByName(worksheet, "单位2数量")].Value?.ToString() ?? string.Empty,CultureInfo.InvariantCulture);
            }

            //Assert
            Assert.Equal(expectedUnit1TotalCounts, acturalUnit1TotalCounts);
            Assert.Equal(expectedUnit2TotalCounts, acturalUnit2TotalCounts);
        }
    }
}
