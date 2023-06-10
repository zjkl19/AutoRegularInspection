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
using System.IO;
using System.Xml.Serialization;

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

            //反序列化XML配置文件
            var serializer = new XmlSerializer(typeof(OptionConfiguration));
            StreamReader reader = new StreamReader($"{App.ConfigurationFolder}\\{App.ConfigFileName}");    //TODO：找不到文件的判断
            var deserializedConfig = (OptionConfiguration)serializer.Deserialize(reader);

            //XElement pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            //XElement pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
            //XElement pictureMaxCompressSize = config.Elements("configuration").Elements("Picture").Elements("MaxCompressSize").FirstOrDefault();
            //XElement pictureCompressQuality = config.Elements("configuration").Elements("Picture").Elements("CompressQuality").FirstOrDefault();

            //XElement compressPictureWidth = config.Elements("configuration").Elements("Picture").Elements("CompressWidth").FirstOrDefault();
            //XElement compressPictureHeight = config.Elements("configuration").Elements("Picture").Elements("CompressHeight").FirstOrDefault();

            //double ImageWidth = ConvertUtil.MillimeterToPoint(Convert.ToDouble(pictureWidth.Value, CultureInfo.InvariantCulture));
            //double ImageHeight = ConvertUtil.MillimeterToPoint(Convert.ToDouble(pictureHeight.Value, CultureInfo.InvariantCulture));
            //double CompressImageWidth = Convert.ToDouble(compressPictureWidth.Value, CultureInfo.InvariantCulture); double CompressImageHeight = Convert.ToDouble(compressPictureHeight.Value, CultureInfo.InvariantCulture);

            string templateFile = $"{ App.ReportTemplatesFolder}\\{App.TemplateFileList[TemplateFileComboBox.SelectedIndex].Name}";

            string outputFile = App.OutputReportFileName;

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;


            //OptionWindowHelper.ExtractSummaryTableWidth(config, out BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth, out SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth, out SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth);


            GenerateReportSettings generateReportSettings = new GenerateReportSettings
            {
                ComboBoxReportTemplates = App.TemplateFileList[TemplateFileComboBox.SelectedIndex]
                ,
                ImageSettings = new ImageSettings
                {
                    MaxCompressSize = deserializedConfig.Picture.MaxCompressSize
                    ,
                    CompressQuality = deserializedConfig.Picture.CompressQuality
                    ,
                    CompressImageWidth = ConvertUtil.MillimeterToPoint(deserializedConfig.Picture.Width)
                    ,
                    CompressImageHeight = ConvertUtil.MillimeterToPoint(deserializedConfig.Picture.Height)
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
                IntactStructNoInsertSummaryTable = deserializedConfig.General.IntactStructNoInsertSummaryTable
                ,
                BookmarkSettings = new BookmarkSettings
                {
                    BridgeDeckBookmarkStartNo = deserializedConfig.Bookmark.BridgeDeckBookmarkStartNo,
                    SuperSpaceBookmarkStartNo = deserializedConfig.Bookmark.SuperSpaceBookmarkStartNo,
                    SubSpaceBookmarkStartNo = deserializedConfig.Bookmark.SubSpaceBookmarkStartNo
                },
                BridgeDeckTableCellWidth = new TableCellWidth
                {
                    No = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.No),
                    Position = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.Position),
                    Component = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.Component),
                    Damage = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.Damage)
                    ,
                    DamagePosition = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.DamagePosition)
                    ,
                    DamageDescription = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.DamageDescription),
                    PictureNo = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.PictureNo),
                    Comment = ConvertUtil.MillimeterToPoint(deserializedConfig.BridgeDeckSummaryTable.Comment)
                }
                ,
                SuperSpaceTableCellWidth = new TableCellWidth
                {
                    No = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.No),
                    Position = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.Position),
                    Component = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.Component),
                    Damage = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.Damage)
                ,
                    DamagePosition = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.DamagePosition)
                ,
                    DamageDescription = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.DamageDescription),
                    PictureNo = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.PictureNo),
                    Comment = ConvertUtil.MillimeterToPoint(deserializedConfig.SuperSpaceSummaryTable.Comment)
                }
                ,
                SubSpaceTableCellWidth = new TableCellWidth
                {
                    No = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.No),
                    Position = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.Position),
                    Component = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.Component),
                    Damage = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.Damage),
                    DamagePosition = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.DamagePosition),
                    DamageDescription = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.DamageDescription),
                    PictureNo = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.PictureNo),
                    Comment = ConvertUtil.MillimeterToPoint(deserializedConfig.SubSpaceSummaryTable.Comment)
                }
            };

            new Thread(() =>
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                {
                    GenerateReport(generateReportSettings, commentColumnInsertTable, ConvertUtil.MillimeterToPoint(deserializedConfig.Picture.Width), ConvertUtil.MillimeterToPoint(deserializedConfig.Picture.Height), templateFile, outputFile, generateReportSettings.ImageSettings.CompressQuality, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);

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

            DamageSummaryServices.InitListDamageSummary1(l1,generateReportSettings.BookmarkSettings.BridgeDeckBookmarkStartNo);
            DamageSummaryServices.InitListDamageSummary1(l2, generateReportSettings.BookmarkSettings.SuperSpaceBookmarkStartNo);
            DamageSummaryServices.InitListDamageSummary1(l3, generateReportSettings.BookmarkSettings.SubSpaceBookmarkStartNo);

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
