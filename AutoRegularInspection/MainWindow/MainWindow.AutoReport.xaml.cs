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
using System.Globalization;
using AutoRegularInspection.Views;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window

    {
        private void AutoReport_Click(object sender, RoutedEventArgs e)
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool commentColumnInsertTable;
            commentColumnInsertTable = Convert.ToBoolean(appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value, CultureInfo.InvariantCulture);

            XDocument config = XDocument.Load(App.ConfigFileName);
            XElement pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            XElement pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();

            double ImageWidth = Convert.ToDouble(pictureWidth.Value, CultureInfo.InvariantCulture); double ImageHeight = Convert.ToDouble(pictureHeight.Value, CultureInfo.InvariantCulture);

            string TemplateFile=
                BridgeDeckGrid.DataContext
            string templateFile = App.TemplateReportFileName; string outputFile = App.OutputReportFileName;

            int CompressImageFlag = 80;    //图片压缩质量（0-100,值越大质量越高）

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;


            OptionWindowHelper.ExtractSummaryTableWidth(config, out BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth, out SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth, out SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth);

            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                InspectionString="检测"
                ,
                DeletePositionInBridgeDeckCheckBox = Convert.ToBoolean(appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value, CultureInfo.InvariantCulture)
                ,
                CustomTableCellWidth = Convert.ToBoolean(appConfig.AppSettings.Settings["CustomSummaryTableWidth"].Value, CultureInfo.InvariantCulture)
                ,
                IntactStructNoInsertSummaryTable = Convert.ToBoolean(config.Elements("configuration").Elements("General").Elements("IntactStructNoInsertSummaryTable").FirstOrDefault().Value, CultureInfo.InvariantCulture)
                ,
                BridgeDeckTableCellWidth = new TableCellWidth { No = bridgeDeckDamageSummaryTableWidth.No, Position = bridgeDeckDamageSummaryTableWidth.Position, Component = bridgeDeckDamageSummaryTableWidth.Component, Damage = bridgeDeckDamageSummaryTableWidth.Damage, DamageDescription = bridgeDeckDamageSummaryTableWidth.DamageDescription, PictureNo = bridgeDeckDamageSummaryTableWidth.PictureNo, Comment = bridgeDeckDamageSummaryTableWidth.Comment }
                ,
                SuperSpaceTableCellWidth = new TableCellWidth { No = superSpaceDamageSummaryTableWidth.No, Position = superSpaceDamageSummaryTableWidth.Position, Component = superSpaceDamageSummaryTableWidth.Component, Damage = superSpaceDamageSummaryTableWidth.Damage, DamageDescription = superSpaceDamageSummaryTableWidth.DamageDescription, PictureNo = superSpaceDamageSummaryTableWidth.PictureNo, Comment = superSpaceDamageSummaryTableWidth.Comment }
                ,
                SubSpaceTableCellWidth = new TableCellWidth { No = subSpaceDamageSummaryTableWidth.No, Position = subSpaceDamageSummaryTableWidth.Position, Component = subSpaceDamageSummaryTableWidth.Component, Damage = subSpaceDamageSummaryTableWidth.Damage, DamageDescription = subSpaceDamageSummaryTableWidth.DamageDescription, PictureNo = subSpaceDamageSummaryTableWidth.PictureNo, Comment = subSpaceDamageSummaryTableWidth.Comment }
            };

            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    GenerateReport(generateReportSettings, commentColumnInsertTable, ImageWidth, ImageHeight, templateFile, outputFile, CompressImageFlag, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);

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

        private static void GenerateReport(GenerateReportSettings generateReportSettings, bool CommentColumnInsertTable, double ImageWidth, double ImageHeight, string templateFile, string outputFile, int CompressImageFlag, ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary, ObservableCollection<DamageSummary> _superSpaceListDamageSummary, ObservableCollection<DamageSummary> _subSpaceListDamageSummary)
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
                //progressBarModel.ProgressValue = 0;    //测试数据
                //生成报告前先验证照片的有效性
                int totalInvalidPictureCounts = PictureServices.ValidatePictures(l1, l2, l3, out List<string> bridgeDeckValidationResult, out List<string> superSpaceValidationResult, out List<string> subSpaceValidationResult);
                if (totalInvalidPictureCounts > 0)
                {
                    try
                    {
                        WriteInvalidPicturesResultToTxt(totalInvalidPictureCounts, bridgeDeckValidationResult, superSpaceValidationResult, subSpaceValidationResult);
                        MessageBox.Show($"存在无效照片，无法生成报告，共计{totalInvalidPictureCounts}张，详见根目录{App.InvalidPicturesStoreFile}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        //throw;
                    }
                }

                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Show(); });
                Document doc = new Document(templateFile);
                var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, l1, l2, l3);
                asposeService.GenerateReport(ref progressBarModel, CommentColumnInsertTable, ImageWidth, ImageHeight, CompressImageFlag);

                doc.Save(outputFile, SaveFormat.Docx);

                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("成功生成报告！"); });

            }));
            thread.Start();

        }
    }
}
