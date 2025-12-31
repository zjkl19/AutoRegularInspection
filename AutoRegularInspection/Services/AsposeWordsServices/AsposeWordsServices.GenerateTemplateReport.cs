using Aspose.Words;
using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public partial class AsposeWordsServices
    {
        public void GenerateGeneralTemplateReport(ProgressBarModel progressModel)
        {
            // 获取要处理的总数量
            int totalParts = _damageSummaries.Count;
            int processedParts = 0;
            // 从指定书签开始插入损坏摘要
            InsertBookmarksAtBookmark("SuperSpaceStart", _damageSummaries);
            // 遍历字典中的每个部分
            foreach (var kvp in _damageSummaries)
            {
                string partName = kvp.Key;
                List<DamageSummary> damageSummaries = kvp.Value;

                // 更新进度
                int progress = (int)((processedParts / (double)totalParts) * 100);
                progressModel.ReportProgress($"正在处理{partName}……", progress);

                // 处理每个部分的数据
                try
                {   
                    //InsertSummaryAndPictureTable(partName, damageSummaries);
                    InsertSummaryAndPictureTableWithBuilder(partName, damageSummaries);
                    
                }
                catch (Exception)
                {
                    // 可以添加日志记录或其他异常处理逻辑
                }                
                System.Threading.Thread.Sleep(1000);    // 模拟处理时间
                processedParts++;    // 更新已处理部分计数
            }

            progressModel.ReportProgress("正在生成统计汇总表…", 98);
            try
            {
                //CreateStatisticsTable();
                //CreateStatisticsTableWithPosition();
                CreateStatisticsTableWithPosition(_damageSummaries);
            }
            catch (Exception)
            {
                // 可以添加日志记录或其他异常处理逻辑
            }
            System.Threading.Thread.Sleep(1000);

            // 更新进度到99%
            progressModel.ReportProgress("正在替换文档变量…", 99);
            try
            {
                ReplaceDocVariable();
            }
            catch (Exception)
            {
                // 可以添加日志记录或其他异常处理逻辑
            }

            // 插入总结文字
            try
            {
                InsertSummaryWords();
            }
            catch (Exception)
            {
                // 可以添加日志记录或其他异常处理逻辑
            }

            // 更新文档字段
            try
            {
                _doc.UpdateFields();
            }
            catch (Exception)
            {
                // 可以添加日志记录或其他异常处理逻辑
            }

            // 更新进度到100%
            progressModel.ProgressValue = 100;
            progressModel.Content = "正在完成……";

        }
        public void GenerateDefaultTemplateReport(ProgressBarModel progressModel)
        {
            //progressModel.ProgressValue = 0;
            //progressModel.Content = $"正在处理{Properties.Resources.BridgeDeck}……";
            progressModel.ReportProgress($"正在处理{Properties.Resources.BridgeDeck}……", 0);
            try
            {
                InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, _bridgeDeckListDamageSummary);
            }
            catch (Exception)
            {

                ;
            }
            System.Threading.Thread.Sleep(1000);

            //progressModel.Content = $"正在处理{Properties.Resources.SuperSpace}……";
            //progressModel.ProgressValue = 33;
            progressModel.ReportProgress($"正在处理{Properties.Resources.SuperSpace}……", 33);
            try
            {
                InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, _superSpaceListDamageSummary);
            }
            catch (Exception)
            {

                ;
            }

            System.Threading.Thread.Sleep(1000);


            //progressModel.Content = $"正在处理{Properties.Resources.SubSpace}……";
            //progressModel.ProgressValue = 66;
            progressModel.ReportProgress($"正在处理{Properties.Resources.SubSpace}……", 66);
            try
            {
                InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, _subSpaceListDamageSummary);
            }
            catch (Exception)
            {

                ;
            }

            System.Threading.Thread.Sleep(1000);

            //默认模板没有统计汇总表
            //progressModel.Content = "正在生成统计汇总表……";
            //progressModel.ProgressValue = 90;
            //progressModel.ReportProgress("正在生成统计汇总表…", 90);
            //CreateStatisticsTable();
            //System.Threading.Thread.Sleep(1000);


            //progressModel.Content = "正在替换文档变量……";
            //progressModel.ProgressValue = 99;
            progressModel.ReportProgress("正在替换文档变量…", 99);

            try
            {
                ReplaceDocVariable();
            }
            catch (Exception)
            {

                ;
            }
            //其它不怎么耗时的操作
            try
            {
                InsertSummaryWords();
            }
            catch (Exception)
            {

                ;
            }

            try
            {
                //两次更新域，1次更新序号，1次更新序号对应的交叉引用
                _doc.UpdateFields();
                _doc.UpdateFields();
            }
            catch (Exception)
            {

                ;
            }

            progressModel.ProgressValue = 100;
            progressModel.Content = "正在完成……";
        }

        public void GenerateJinan2023TemplateReport(ProgressBarModel progressModel)
        {
            progressModel.ReportProgress($"正在处理{Properties.Resources.BridgeDeck}……", 0);
            try
            {
                InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, _bridgeDeckListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress($"正在处理{Properties.Resources.SuperSpace}……", 33);
            try
            {
                InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, _superSpaceListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress($"正在处理{Properties.Resources.SubSpace}……", 66);

            try
            {
                InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, _subSpaceListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress("正在生成统计汇总表…", 90);
            try
            {
                CreateStatisticsTable();
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress("正在替换文档变量…", 99);
            try
            {
                ReplaceDocVariable();
            }
            catch (Exception)
            {

                ;
            }

            try
            {
                InsertSummaryWords();
            }
            catch (Exception)
            {

                ;
            }
            try
            {
                _doc.UpdateFields();
                _doc.UpdateFields();
            }
            catch (Exception)
            {

                ;
            }

            progressModel.ProgressValue = 100;
            progressModel.Content = "正在完成……";
        }

        public void GenerateCityBridge2024TemplateReport(ProgressBarModel progressModel)
        {
            progressModel.ReportProgress($"正在处理{Properties.Resources.BridgeDeck}……", 0);
            try
            {
                InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, _bridgeDeckListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress($"正在处理{Properties.Resources.SuperSpace}……", 33);
            try
            {
                InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, _superSpaceListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress($"正在处理{Properties.Resources.SubSpace}……", 66);

            try
            {
                InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, _subSpaceListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress("正在生成统计汇总表…", 90);
            try
            {
                CreateStatisticsTable();
                CreateStatisticsTableWithPosition();
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress("正在替换文档变量…", 99);
            try
            {
                ReplaceDocVariable();
            }
            catch (Exception)
            {

                ;
            }

            try
            {
                InsertSummaryWords();
            }
            catch (Exception)
            {

                ;
            }
            try
            {
                _doc.UpdateFields();
                _doc.UpdateFields();
            }
            catch (Exception)
            {

                ;
            }

            progressModel.ProgressValue = 100;
            progressModel.Content = "正在完成……";
        }
        public void GenerateTransportationTemplateReport(ProgressBarModel progressModel)
        {
            progressModel.ReportProgress($"正在处理{Properties.Resources.BridgeDeck}……", 0);
            try
            {
                InsertSummaryAndPictureTable(BridgeDeckBookmarkStartName, _bridgeDeckListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress($"正在处理{Properties.Resources.SuperSpace}……", 33);
            try
            {
                InsertSummaryAndPictureTable(SuperSpaceBookmarkStartName, _superSpaceListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress($"正在处理{Properties.Resources.SubSpace}……", 66);

            try
            {
                InsertSummaryAndPictureTable(SubSpaceBookmarkStartName, _subSpaceListDamageSummary);
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress("正在生成统计汇总表…", 90);
            try
            {
                CreateStatisticsTable();
            }
            catch (Exception) {; }
            System.Threading.Thread.Sleep(1000);

            progressModel.ReportProgress("正在替换文档变量…", 99);
            try
            {
                ReplaceDocVariable();
            }
            catch (Exception)
            {

                ;
            }

            try
            {
                InsertSummaryWords();
            }
            catch (Exception) {; }
            try
            {
                _doc.UpdateFields();
                _doc.UpdateFields();
            }
            catch (Exception)
            {

                ;
            }


            progressModel.ProgressValue = 100;
            progressModel.Content = "正在完成……";
        }
    }
}
