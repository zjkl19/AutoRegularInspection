using Aspose.Words;
using System;
using Xunit;
using AutoRegularInspection.Services;
using AutoRegularInspection.Models;
using AutoRegularInspection.ViewModels;
using System.Linq;
using AutoRegularInspection.IRepository;
using Ninject;
using System.Collections.Generic;
using Aspose.Words.Tables;

namespace AutoRegularInspectionTestProject
{
    public class MainWindowTests
    {
        //相当于集成测试
        [Fact]
        public void GenerateReport_Test()
        {
            //Arrange
            //string fileName = @"..\..\..\TestFiles\FindUnitError.doc";
            string templateFile = @"..\..\..\..\AutoRegularInspectionTestProject\TestFiles\外观检查报告模板.docx";
            string outputFile = @"..\..\..\..\AutoRegularInspectionTestProject\TestFiles\外观检查报告.docx";
            double ImageWidth = 224.25; double ImageHeight = 168.75; int CompressImageFlag = 80;

            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            List<DamageSummary> l1,l2,l3;

            l1 = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            DamageSummaryServices.InitListDamageSummary(l1);
            l2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);

            DamageSummaryServices.InitListDamageSummary(l2, 2_000_000, BridgePart.SuperSpace);

            l3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(l3, 3_000_000, BridgePart.SubSpace);

            //Act
            var doc = new Document(templateFile);
            var asposeService = new AsposeWordsServices(ref doc, l1, l2, l3);
            asposeService.GenerateSummaryTableAndPictureTable(ImageWidth, ImageHeight, CompressImageFlag);

            doc.UpdateFields();
            doc.UpdateFields();

            //参考算法
            //var bridgeName = string.Empty;
            //NodeCollection allTables = _doc.GetChildNodes(NodeType.Table, true);
            //for (int i = 0; i < allTables.Count; i++)
            //{
            //    Table table0 = _doc.GetChildNodes(NodeType.Table, true)[i] as Table;
            //    if ((table0.Rows[0].Cells[0].GetText().IndexOf("委托单位") >= 0))
            //    {
            //        Cell cell = table0.Rows[2].Cells[1];
            //        bridgeName = cell.GetText().Replace("\a", "").Replace("\r", "");    //用GetText()的方法来获取cell中的值

            //        break;
            //    }
            //}
            Table bridgeDeckDamageSummaryTable=null; Table bridgeDeckDamagePictureTable = null;
            int bridgeDeckDamageSummaryTableIndex = 0;
            NodeCollection allTables = doc.GetChildNodes(NodeType.Table, true);
            for (int i = 0; i < allTables.Count; i++)
            {
                bridgeDeckDamageSummaryTable = doc.GetChildNodes(NodeType.Table, true)[i] as Table;
                
                if(bridgeDeckDamageSummaryTable.Rows[0].Cells.Count<3)    //防止越界
                {
                    continue;
                }

                if (bridgeDeckDamageSummaryTable.Rows[0].Cells[2].GetText().IndexOf("要素") >= 0)    //先找到桥面系病害的汇总表
                {
                    bridgeDeckDamageSummaryTableIndex = i;
                    bridgeDeckDamagePictureTable= doc.GetChildNodes(NodeType.Table, true)[i+1] as Table;
                    break;
                }
            }

            doc.UnlinkFields();   //看情况决定是否要解除链接
            doc.Save(outputFile, SaveFormat.Docx);   //如果需要查看生成的文件，则加上这句

            //Assert
            
            //TODO:测试插入的图片是否正确

            //测试汇总表
            Assert.Contains("缝内沉积物阻塞", bridgeDeckDamageSummaryTable.Rows[1].Cells[3].GetText(), StringComparison.CurrentCulture);
            Assert.Contains("接缝处铺装碎边", bridgeDeckDamageSummaryTable.Rows[2].Cells[3].GetText(), StringComparison.CurrentCulture);
            Assert.Contains("图 2-3",bridgeDeckDamageSummaryTable.Rows[2].Cells[5].GetText().Trim(),StringComparison.CurrentCulture);

            //测试汇总图片表  
            Assert.Contains("图 2-1 左幅0#伸缩缝沉积物阻塞-1", bridgeDeckDamagePictureTable.Rows[1].Cells[0].GetText().Trim(), StringComparison.CurrentCulture);
            Assert.Contains("图 2-4 右幅1#伸缩缝沉积物阻塞-1", bridgeDeckDamagePictureTable.Rows[3].Cells[1].GetText().Trim(), StringComparison.CurrentCulture);

        }
    }
}
