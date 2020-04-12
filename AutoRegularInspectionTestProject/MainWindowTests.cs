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
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace AutoRegularInspectionTestProject
{
    public class MainWindowTests
    {
        //相当于集成测试
        [Fact]
        public void GenerateReportIntegratedTest()
        {
            //Arrange

            const string tempPath = "Temp";
            string templateFile = @"TestFiles\外观检查报告模板.docx";
            string outputFile = @"TestFiles\外观检查报告.docx";


            double ImageWidth = 224.25; double ImageHeight = 168.75; int CompressImageFlag = 80;

            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            List<DamageSummary> l1, l2, l3;

            l1 = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            DamageSummaryServices.InitListDamageSummary(l1);
            l2 = dataRepository.ReadDamageData(BridgePart.SuperSpace);

            DamageSummaryServices.InitListDamageSummary(l2, 2_000_000, BridgePart.SuperSpace);

            l3 = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(l3, 3_000_000, BridgePart.SubSpace);

            //Act
            var doc = new Document(templateFile);
            var asposeService = new AsposeWordsServices(ref doc, l1, l2, l3);
            asposeService.GenerateSummaryTableAndPictureTable(false,ImageWidth, ImageHeight, CompressImageFlag);

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
            Table bridgeDeckDamageSummaryTable = null; Table bridgeDeckDamagePictureTable = null;
            int bridgeDeckDamageSummaryTableIndex = 0;
            NodeCollection allTables = doc.GetChildNodes(NodeType.Table, true);
            for (int i = 0; i < allTables.Count; i++)
            {
                bridgeDeckDamageSummaryTable = doc.GetChildNodes(NodeType.Table, true)[i] as Table;

                if (bridgeDeckDamageSummaryTable.Rows[0].Cells.Count < 3)    //防止越界
                {
                    continue;
                }

                if (bridgeDeckDamageSummaryTable.Rows[0].Cells[2].GetText().IndexOf("要素", StringComparison.Ordinal) >= 0)    //先找到桥面系病害的汇总表
                {
                    bridgeDeckDamageSummaryTableIndex = i;
                    bridgeDeckDamagePictureTable = doc.GetChildNodes(NodeType.Table, true)[i + 1] as Table;
                    break;
                }
            }

            Table superSpaceDamageSummaryTable = doc.GetChildNodes(NodeType.Table, true)[bridgeDeckDamageSummaryTableIndex + 2] as Table;
            Table superSpaceDamagePictureTable = doc.GetChildNodes(NodeType.Table, true)[bridgeDeckDamageSummaryTableIndex + 2 + 1] as Table;

            Table subSpaceDamageSummaryTable = doc.GetChildNodes(NodeType.Table, true)[bridgeDeckDamageSummaryTableIndex + 2 * 2] as Table;
            Table subSpaceDamagePictureTable = doc.GetChildNodes(NodeType.Table, true)[bridgeDeckDamageSummaryTableIndex + 2 * 2 + 1] as Table;

            doc.UnlinkFields();   //看情况决定是否要解除链接
            doc.Save(outputFile, SaveFormat.Docx);   //如果需要查看生成的文件，则加上这句


            int skipBefore = 22; int skipAfter = 28;    //该数据要手动测试出来
            List<string> fileNameList = AutoRegularInspection.Repository.AsposeWordsImage.ExportImageFromWordFile(skipBefore, skipAfter,outputFile, @$"{tempPath}\").ToList();

            //Assert
            //测试汇总内容
            Assert.Contains("伸缩缝：共3条缝内沉积物阻塞，长度29.8米；共1处接缝处铺装碎边，面积0.6平方米。\r栏杆：共1处丢失残缺。", doc.Range.Text,StringComparison.Ordinal);
            Assert.Contains("台身：共3处露筋锈蚀，面积0.48平方米", doc.Range.Text, StringComparison.Ordinal);

            //测试汇总表（桥面系）
            Assert.Contains("缝内沉积物阻塞", bridgeDeckDamageSummaryTable.Rows[1].Cells[3].GetText(), StringComparison.Ordinal);
            Assert.Contains("接缝处铺装碎边", bridgeDeckDamageSummaryTable.Rows[2].Cells[3].GetText(), StringComparison.Ordinal);
            Assert.Contains("图 2-3", bridgeDeckDamageSummaryTable.Rows[2].Cells[5].GetText().Trim(), StringComparison.Ordinal);
            //测试汇总图片表（桥面系）
            Assert.Contains("图 2-1 左幅0#伸缩缝沉积物阻塞-1", bridgeDeckDamagePictureTable.Rows[1].Cells[0].GetText().Trim(), StringComparison.Ordinal);
            Assert.Contains("图 2-4 右幅1#伸缩缝沉积物阻塞-1", bridgeDeckDamagePictureTable.Rows[3].Cells[1].GetText().Trim(), StringComparison.Ordinal);
            //测试图片md5（桥面系）
            Assert.Equal("589279c71781666ad3adf2c76a71c9e7", GetFileMD5(@$"{tempPath}\{fileNameList[0]}"),true);
            Assert.Equal("8ec8d8aa1883810e1c4141066b67ca0d", GetFileMD5(@$"{tempPath}\{fileNameList[1]}"), true);

            //测试汇总表（上部结构）
            Assert.Contains("无", superSpaceDamageSummaryTable.Rows[1].Cells[3].GetText(), StringComparison.Ordinal);
            Assert.Contains("图 2-10", superSpaceDamageSummaryTable.Rows[2].Cells[5].GetText().Trim(), StringComparison.Ordinal);
            //测试汇总图片表（上部结构）
            Assert.Contains("图 2-9 左幅主梁", superSpaceDamagePictureTable.Rows[1].Cells[0].GetText().Trim(), StringComparison.Ordinal);
            Assert.Contains("图 2-10 右幅主梁", superSpaceDamagePictureTable.Rows[1].Cells[1].GetText().Trim(), StringComparison.Ordinal);
            //测试图片md5（上部结构）
            Assert.Equal("589279c71781666ad3adf2c76a71c9e7", GetFileMD5(@$"{tempPath}\{fileNameList[8]}"), true);
            Assert.Equal("22afdccd0a7b3334c8b3ab811ce5c192", GetFileMD5(@$"{tempPath}\{fileNameList[9]}"), true);

            //测试汇总表（下部结构）
            Assert.Contains("水蚀", subSpaceDamageSummaryTable.Rows[1].Cells[3].GetText(), StringComparison.Ordinal);
            Assert.Contains("图 2-12", subSpaceDamageSummaryTable.Rows[2].Cells[5].GetText().Trim(), StringComparison.Ordinal);
            //测试汇总图片表（下部结构）
            Assert.Contains("图 2-13 左幅1#台台身露筋锈蚀", subSpaceDamagePictureTable.Rows[1+2].Cells[0].GetText().Trim(), StringComparison.Ordinal);
            Assert.Contains("图 2-14 右幅1#台台身露筋锈蚀", subSpaceDamagePictureTable.Rows[1+2].Cells[1].GetText().Trim(), StringComparison.Ordinal);
            //测试图片md5（下部结构）
            Assert.Equal("8ec8d8aa1883810e1c4141066b67ca0d", GetFileMD5(@$"{tempPath}\{fileNameList[12]}"), true);
            Assert.Equal("4f23222732ec6519c38713614379ae11", GetFileMD5(@$"{tempPath}\{fileNameList[13]}"), true);
        }

        public static string GetFileMD5(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            int bufferSize = 1048576;
            byte[] buff = new byte[bufferSize];
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();
            long offset = 0;
            while (offset < fs.Length)
            {
                long readSize = bufferSize;
                if (offset + readSize > fs.Length)
                    readSize = fs.Length - offset;
                fs.Read(buff, 0, Convert.ToInt32(readSize));
                if (offset + readSize < fs.Length)
                    md5.TransformBlock(buff, 0, Convert.ToInt32(readSize), buff, 0);
                else
                    md5.TransformFinalBlock(buff, 0, Convert.ToInt32(readSize));
                offset += bufferSize;
            }
            if (offset >= fs.Length)
            {
                fs.Close();
                byte[] result = md5.Hash;
                md5.Clear();
                StringBuilder sb = new StringBuilder(32);
                for (int i = 0; i < result.Length; i++)
                    sb.Append(result[i].ToString("X2"));
                return sb.ToString();
            }
            else
            {
                fs.Close();
                return null;
            }
        }
    }
}
