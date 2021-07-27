using Aspose.Words;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window

    {
        private void AutoReport_Click(object sender, RoutedEventArgs e)
        {
            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool commentColumnInsertTable;
            commentColumnInsertTable = Convert.ToBoolean(appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value);

            var config = XDocument.Load(App.ConfigFileName);
            var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();

            //double ImageWidth = 224.25; double ImageHeight = 168.75;
            double ImageWidth = Convert.ToDouble(pictureWidth.Value.ToString()); double ImageHeight = Convert.ToDouble(pictureHeight.Value.ToString());

            string templateFile = App.TemplateReportFileName; string outputFile = App.OutputReportFileName;

            int CompressImageFlag = 80;    //图片压缩质量（0-100,值越大质量越高）

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;



            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    GenerateReport(commentColumnInsertTable, ImageWidth, ImageHeight, templateFile, outputFile, CompressImageFlag, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);

                    //try
                    //{
                    //    GenerateReport(ImageWidth, ImageHeight, templateFile, outputFile, CompressImageFlag, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);


                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                }));
            }).Start();


        }

        private static void GenerateReport(bool CommentColumnInsertTable, double ImageWidth, double ImageHeight, string templateFile, string outputFile, int CompressImageFlag, ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary, ObservableCollection<DamageSummary> _superSpaceListDamageSummary, ObservableCollection<DamageSummary> _subSpaceListDamageSummary)
        {
            var w = new ProgressBarWindow();
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.4 * (App.ScreenWidth - w.Width);

            var progressBarModel = new ProgressBarModel
            {
                ProgressValue = 0
            };
            w.progressBarNumberTextBlock.DataContext = progressBarModel;
            w.progressBar.DataContext = progressBarModel;
            w.progressBarContentTextBlock.DataContext = progressBarModel;

            var progressSleepTime = 500;    //进度条停顿时间

            List<DamageSummary> l1 = _bridgeDeckListDamageSummary.ToList();
            List<DamageSummary> l2 = _superSpaceListDamageSummary.ToList();
            List<DamageSummary> l3 = _subSpaceListDamageSummary.ToList();

            DamageSummaryServices.InitListDamageSummary1(l1);
            DamageSummaryServices.InitListDamageSummary1(l2, 2_000_000);
            DamageSummaryServices.InitListDamageSummary1(l3, 3_000_000);



            var thread = new Thread(new ThreadStart(() =>
            {
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Show(); });
                //progressBarModel.ProgressValue = 0;    //测试数据

                var doc = new Document(templateFile);
                var asposeService = new AsposeWordsServices(ref doc, l1, l2, l3);
                asposeService.GenerateSummaryTableAndPictureTable(ref progressBarModel, CommentColumnInsertTable, ImageWidth, ImageHeight, CompressImageFlag);

                //两次更新域，1次更新序号，1次更新序号对应的交叉引用
                doc.UpdateFields();
                doc.UpdateFields();

                doc.Save(outputFile, SaveFormat.Docx);
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("成功生成报告！"); });

            }));
            thread.Start();

        }
    }
}
