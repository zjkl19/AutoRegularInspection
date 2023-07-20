using Aspose.Words.Tables;
using Aspose.Words;
using AutoRegularInspection;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AutoRegularInspection.IRepository;
using System.Globalization;
using Ninject;
using Moq;
using SixLabors.ImageSharp.PixelFormats;
using System.Linq;
using Aspose.Words.Fields;

namespace AutoRegularInspectionTestProject.Services
{
    public class AsposeWordsServicesTests
    {
        [Fact]
        public void GetTotalPictureCounts_EmptyList_ReturnsZero()
        {
            // Arrange
            var listDamageSummary = new List<DamageSummary>();

            // Act
            int result = AsposeWordsServices.GetTotalPictureCounts(listDamageSummary);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTotalPictureCounts_SingleElement_ReturnsElementPictureCounts()
        {
            // Arrange
            var listDamageSummary = new List<DamageSummary>
        {
            new DamageSummary { PictureCounts = 5 }
        };

            // Act
            int result = AsposeWordsServices.GetTotalPictureCounts(listDamageSummary);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetTotalPictureCounts_MultipleElements_ReturnsSumOfElementPictureCounts()
        {
            // Arrange
            var listDamageSummary = new List<DamageSummary>
        {
            new DamageSummary { PictureCounts = 5 },
            new DamageSummary { PictureCounts = 3 },
            new DamageSummary { PictureCounts = 7 }
        };

            // Act
            int result = AsposeWordsServices.GetTotalPictureCounts(listDamageSummary);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void GetSummaryTableBookmarkValue_BridgeDeckBookmarkStartName_ReturnsCorrectValue()
        {
            // Arrange
            var bookmarkStartName = AsposeWordsServices.BridgeDeckBookmarkStartName;

            // Act
            string result = AsposeWordsServices.GetSummaryTableBookmarkValue(bookmarkStartName);

            // Assert
            Assert.Equal($"_Ref{App.TableRefOffset + 1}", result);
        }

        [Fact]
        public void GetSummaryTableBookmarkValue_SuperSpaceBookmarkStartName_ReturnsCorrectValue()
        {
            // Arrange
            var bookmarkStartName = AsposeWordsServices.SuperSpaceBookmarkStartName;

            // Act
            string result = AsposeWordsServices.GetSummaryTableBookmarkValue(bookmarkStartName);

            // Assert
            Assert.Equal($"_Ref{App.TableRefOffset + 2}", result);
        }

        [Fact]
        public void GetSummaryTableBookmarkValue_OtherBookmarkStartName_ReturnsCorrectValue()
        {
            // Arrange
            var bookmarkStartName = "OtherBookmarkStartName";

            // Act
            string result = AsposeWordsServices.GetSummaryTableBookmarkValue(bookmarkStartName);

            // Assert
            Assert.Equal($"_Ref{App.TableRefOffset + 3}", result);
        }


        [Fact]
        public void CreateTable_CreatesTableWithExpectedProperties()
        {
            // Arrange
            List<DamageSummary> l1, l2, l3;
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            l1 = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            DamageSummaryServices.InitListDamageSummary(l1);
            l2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);

            DamageSummaryServices.InitListDamageSummary(l2, 2_000_000, BridgePart.SuperSpace);

            l3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(l3, 3_000_000, BridgePart.SubSpace);

            //Act
            var doc = new Document();
            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                ComboBoxReportTemplates = new ComboBoxReportTemplates { DisplayName = "建研报告模板", Name = "外观检查报告模板.docx", DocStyleOfMainText = "迪南自动报告正文", DocStyleOfTable = "迪南自动报告表格", DocStyleOfPicture = "迪南自动报告图片" }
                ,
                InspectionString = "检测"
                ,
                ImageSettings = new ImageSettings
                {
                    MaxCompressSize = Convert.ToInt32(200, CultureInfo.InvariantCulture)
                    ,
                    CompressQuality = Convert.ToInt32(80, CultureInfo.InvariantCulture)
                    ,
                    CompressImageWidth = Convert.ToInt32(224.25)
                    ,
                    CompressImageHeight = Convert.ToInt32(168.75)
                }
                ,
                DeletePositionInBridgeDeckCheckBox = false
          ,
                CustomTableCellWidth = false
          ,
                BridgeDeckTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SuperSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SubSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
            };
            var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, l1, l2, l3);

            var builder = new DocumentBuilder();
            CellFormat cellFormat = builder.CellFormat;
            int totalPictureCounts = 5;

            // Act
            var result = asposeService.CreateTable(totalPictureCounts, builder, cellFormat);

            // Assert
            Assert.Equal(6, result.Rows.Count); // Assumes that the number of rows is 2 * ((totalPictureCounts + 1) / 2)
            Assert.Equal(2, result.FirstRow.Cells.Count); // Assumes that the number of columns is 2

        }

        [Fact]
        public void InsertPictures_InsertsExpectedNumberOfPictures()
        {
            // Arrange
            var mockFileRepository = new Mock<IFileRepository>();
            mockFileRepository.Setup(fr => fr.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(new string[0]);
            mockFileRepository.Setup(fr => fr.Exists(It.IsAny<string>())).Returns(false);
            mockFileRepository.Setup(fr => fr.LoadImage(It.IsAny<string>())).Returns(new SixLabors.ImageSharp.Image<Rgba32>(1, 1));
            mockFileRepository.Setup(fr => fr.GetFileName(It.IsAny<string>(), It.IsAny<string>())).Returns("test.jpg");

            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            List<DamageSummary> l1,l2,l3;
            l1 = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            DamageSummaryServices.InitListDamageSummary(l1);

            l2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            DamageSummaryServices.InitListDamageSummary(l2, 2_000_000, BridgePart.SuperSpace);
            l3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(l3, 3_000_000, BridgePart.SubSpace);

            var doc = new Document();
            var builder = new DocumentBuilder();
            CellFormat cellFormat = builder.CellFormat;
            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                ComboBoxReportTemplates = new ComboBoxReportTemplates { DisplayName = "建研报告模板", Name = "外观检查报告模板.docx", DocStyleOfMainText = "迪南自动报告正文", DocStyleOfTable = "迪南自动报告表格", DocStyleOfPicture = "迪南自动报告图片" }
                ,
                InspectionString = "检测"
                ,
                ImageSettings = new ImageSettings
                {
                    MaxCompressSize = Convert.ToInt32(200, CultureInfo.InvariantCulture)
                    ,
                    CompressQuality = Convert.ToInt32(80, CultureInfo.InvariantCulture)
                    ,
                    CompressImageWidth = Convert.ToInt32(224.25)
                    ,
                    CompressImageHeight = Convert.ToInt32(168.75)
                }
                ,
                DeletePositionInBridgeDeckCheckBox = false
          ,
                CustomTableCellWidth = false
          ,
                BridgeDeckTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SuperSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SubSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
            };
            var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, l1, l2, l3);
            int totalPictureCounts = AsposeWordsServices.GetTotalPictureCounts(l1);
            var pictureTable = asposeService.CreateTable(totalPictureCounts, builder, cellFormat);

            //Act
            asposeService.InsertPictures(l1, builder, pictureTable, mockFileRepository.Object);

            // Assert
            mockFileRepository.Verify(fr => fr.LoadImage(It.IsAny<string>()), Times.Exactly(l1.Sum(ds => ds.PictureCounts)));
        }

        [Fact]
        public void InsertPictures_NoPicturesLoaded_WhenPictureFileDoesNotExist()
        {
            // Arrange
            var mockFileRepository = new Mock<IFileRepository>();
            mockFileRepository.Setup(fr => fr.Exists(It.IsAny<string>())).Returns(false);
            List<DamageSummary> l1,l2,l3;
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            l1 = new List<DamageSummary> {
            new DamageSummary{ Damage="测试病害1",DamageDescription="测试病害描述1",PictureCounts=0}
            ,new DamageSummary{ Damage="测试病害2",DamageDescription="测试病害描述2",PictureCounts=0}};

            l2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            DamageSummaryServices.InitListDamageSummary(l2, 2_000_000, BridgePart.SuperSpace);
            l3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(l3, 3_000_000, BridgePart.SubSpace);
            //Act
            var doc = new Document();
            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                ComboBoxReportTemplates = new ComboBoxReportTemplates { DisplayName = "建研报告模板", Name = "外观检查报告模板.docx", DocStyleOfMainText = "迪南自动报告正文", DocStyleOfTable = "迪南自动报告表格", DocStyleOfPicture = "迪南自动报告图片" }
                ,
                InspectionString = "检测"
                ,
                ImageSettings = new ImageSettings
                {
                    MaxCompressSize = Convert.ToInt32(200, CultureInfo.InvariantCulture)
                    ,
                    CompressQuality = Convert.ToInt32(80, CultureInfo.InvariantCulture)
                    ,
                    CompressImageWidth = Convert.ToInt32(224.25)
                    ,
                    CompressImageHeight = Convert.ToInt32(168.75)
                }
                ,
                DeletePositionInBridgeDeckCheckBox = false
          ,
                CustomTableCellWidth = false
          ,
                BridgeDeckTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SuperSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SubSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
            };
            var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, l1, l2, l3);

            var builder = new DocumentBuilder();
            CellFormat cellFormat = builder.CellFormat;
            int totalPictureCounts = AsposeWordsServices.GetTotalPictureCounts(l1);

            // Act
            var pictureTable = asposeService.CreateTable(totalPictureCounts, builder, cellFormat);

            asposeService.InsertPictures(l1, builder, pictureTable, mockFileRepository.Object);

            // Assert
            mockFileRepository.Verify(fr => fr.LoadImage(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void WriteDescriptions_WritesExpectedDescriptions()
        {
            // Arrange
            var mockFileRepository = new Mock<IFileRepository>();
            mockFileRepository.Setup(fr => fr.GetFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(new string[0]);
            mockFileRepository.Setup(fr => fr.Exists(It.IsAny<string>())).Returns(false);
            mockFileRepository.Setup(fr => fr.LoadImage(It.IsAny<string>())).Returns(new SixLabors.ImageSharp.Image<Rgba32>(1, 1));
            mockFileRepository.Setup(fr => fr.GetFileName(It.IsAny<string>(), It.IsAny<string>())).Returns("test.jpg");

            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            List<DamageSummary> l1, l2, l3;
            l1 = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            DamageSummaryServices.InitListDamageSummary(l1);
            l2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            DamageSummaryServices.InitListDamageSummary(l2, 2_000_000, BridgePart.SuperSpace);
            l3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(l3, 3_000_000, BridgePart.SubSpace);

            var doc = new Document();
            var builder = new DocumentBuilder();
            CellFormat cellFormat = builder.CellFormat;
            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                ComboBoxReportTemplates = new ComboBoxReportTemplates { DisplayName = "建研报告模板", Name = "外观检查报告模板.docx", DocStyleOfMainText = "迪南自动报告正文", DocStyleOfTable = "迪南自动报告表格", DocStyleOfPicture = "迪南自动报告图片" }
                ,
                InspectionString = "检测"
                ,
                ImageSettings = new ImageSettings
                {
                    MaxCompressSize = Convert.ToInt32(200, CultureInfo.InvariantCulture)
                    ,
                    CompressQuality = Convert.ToInt32(80, CultureInfo.InvariantCulture)
                    ,
                    CompressImageWidth = Convert.ToInt32(224.25)
                    ,
                    CompressImageHeight = Convert.ToInt32(168.75)
                }
                ,
                DeletePositionInBridgeDeckCheckBox = false
          ,
                CustomTableCellWidth = false
          ,
                BridgeDeckTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SuperSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
          ,
                SubSpaceTableCellWidth = new TableCellWidth { No = 1, Position = 1, Component = 1, Damage = 1, DamageDescription = 1, PictureNo = 1, Comment = 1 }
            };
            var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, l1, l2, l3);
            int totalPictureCounts = AsposeWordsServices.GetTotalPictureCounts(l1);
            var pictureTable = asposeService.CreateTable(totalPictureCounts, builder, cellFormat);

            asposeService.InsertPictures(l1, builder, pictureTable, mockFileRepository.Object);

            var fieldStyleRefBuilder = new FieldBuilder(FieldType.FieldStyleRef);
            fieldStyleRefBuilder.AddArgument(1);
            fieldStyleRefBuilder.AddSwitch(@"\s");

            var pictureFieldSequenceBuilder = new FieldBuilder(FieldType.FieldSequence);
            pictureFieldSequenceBuilder.AddArgument("图");
            pictureFieldSequenceBuilder.AddSwitch(@"\*", "ARABIC");
            pictureFieldSequenceBuilder.AddSwitch(@"\s", "1");

            // Act
            asposeService.WriteDescriptions(l1, builder, fieldStyleRefBuilder, pictureFieldSequenceBuilder, pictureTable);

            // Assert
            Assert.Contains("左幅0#伸缩缝沉积物阻塞-1", pictureTable.Rows[1].Cells[0].GetText());
            Assert.Contains("左幅0#伸缩缝沉积物阻塞-2", pictureTable.Rows[1].Cells[1].GetText());
            Assert.Contains("左幅0#伸缩缝接缝处铺装碎边", pictureTable.Rows[3].Cells[0].GetText());

        }
    }
}
