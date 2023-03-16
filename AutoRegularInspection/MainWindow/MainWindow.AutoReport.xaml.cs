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

            XDocument config = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");
            XElement pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            XElement pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
            XElement pictureMaxCompressSize = config.Elements("configuration").Elements("Picture").Elements("MaxCompressSize").FirstOrDefault();
            XElement pictureCompressQuality = config.Elements("configuration").Elements("Picture").Elements("CompressQuality").FirstOrDefault();

            XElement compressPictureWidth = config.Elements("configuration").Elements("Picture").Elements("CompressWidth").FirstOrDefault();
            XElement compressPictureHeight = config.Elements("configuration").Elements("Picture").Elements("CompressHeight").FirstOrDefault();
           
            double ImageWidth = ConvertUtil.MillimeterToPoint(Convert.ToDouble(pictureWidth.Value, CultureInfo.InvariantCulture));
            double ImageHeight = ConvertUtil.MillimeterToPoint(Convert.ToDouble(pictureHeight.Value, CultureInfo.InvariantCulture));
            double CompressImageWidth = Convert.ToDouble(compressPictureWidth.Value, CultureInfo.InvariantCulture); double CompressImageHeight = Convert.ToDouble(compressPictureHeight.Value, CultureInfo.InvariantCulture);

            string templateFile = $"{ App.ReportTemplatesFolder}\\{App.TemplateFileList[TemplateFileComboBox.SelectedIndex].Name}";

            string outputFile = App.OutputReportFileName;

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;


            OptionWindowHelper.ExtractSummaryTableWidth(config, out BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth, out SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth, out SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth);


            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                ComboBoxReportTemplates = App.TemplateFileList[TemplateFileComboBox.SelectedIndex]
                ,
                ImageSettings = new ImageSettings
                {
                    MaxCompressSize = Convert.ToInt32(pictureMaxCompressSize.Value, CultureInfo.InvariantCulture)
                    ,
                    CompressQuality = Convert.ToInt32(pictureCompressQuality.Value, CultureInfo.InvariantCulture)
                    ,
                    CompressImageWidth = CompressImageWidth
                    ,
                    CompressImageHeight = CompressImageHeight
                }
                ,
                InspectionString = InspectionComboBox.Text
                ,
                DeletePositionInBridgeDeckCheckBox = Convert.ToBoolean(appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value, CultureInfo.InvariantCulture)
                ,
                DeletePositionInSuperSpaceCheckBox = Convert.ToBoolean(appConfig.AppSettings.Settings["DeletePositionInSuperSpace"].Value, CultureInfo.InvariantCulture)
                ,
                CustomTableCellWidth = Convert.ToBoolean(appConfig.AppSettings.Settings["CustomSummaryTableWidth"].Value, CultureInfo.InvariantCulture)
                ,
                IntactStructNoInsertSummaryTable = Convert.ToBoolean(config.Elements("configuration").Elements("General").Elements("IntactStructNoInsertSummaryTable").FirstOrDefault().Value, CultureInfo.InvariantCulture)
                ,
                BridgeDeckTableCellWidth = new TableCellWidth { No = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.No), Position = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.Position), Component = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.Component), Damage = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.Damage)
                ,DamagePosition = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.DamagePosition)
                , DamageDescription = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.DamageDescription), PictureNo = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.PictureNo), Comment = ConvertUtil.MillimeterToPoint(bridgeDeckDamageSummaryTableWidth.Comment) }
                ,
                SuperSpaceTableCellWidth = new TableCellWidth { No = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.No), Position = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.Position), Component = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.Component), Damage = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.Damage)
                ,DamagePosition = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.DamagePosition)
                , DamageDescription = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.DamageDescription), PictureNo = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.PictureNo), Comment = ConvertUtil.MillimeterToPoint(superSpaceDamageSummaryTableWidth.Comment) }
                ,
                SubSpaceTableCellWidth = new TableCellWidth { No = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.No), Position = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.Position), Component = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.Component), Damage = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.Damage)
                ,DamagePosition = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.DamagePosition)
                , DamageDescription = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.DamageDescription), PictureNo = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.PictureNo), Comment = ConvertUtil.MillimeterToPoint(subSpaceDamageSummaryTableWidth.Comment) }
            };

            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    GenerateReport(generateReportSettings, commentColumnInsertTable, ImageWidth, ImageHeight, templateFile, outputFile, generateReportSettings.ImageSettings.CompressQuality, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);

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

                doc.Save(outputFile, SaveFormat.Doc);

                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("成功生成报告！"); });

            }));
            thread.Start();

        }
    }
}
