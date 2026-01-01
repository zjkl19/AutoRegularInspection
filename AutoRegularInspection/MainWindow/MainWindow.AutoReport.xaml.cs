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
        private async void AutoReport_Click(object sender, RoutedEventArgs e)
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool commentColumnInsertTable;
            commentColumnInsertTable = Convert.ToBoolean(appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value, CultureInfo.InvariantCulture);

            XDocument config = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");

            var deserializedConfig = OptionConfigurationLoader.Load();

            if (App.TemplateFileList == null || TemplateFileComboBox.SelectedIndex < 0 || TemplateFileComboBox.SelectedIndex >= App.TemplateFileList.Count)
            {
                MessageBox.Show("未选择有效模板，请检查 templates.json。", "模板校验失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedTemplate = App.TemplateFileList[TemplateFileComboBox.SelectedIndex];
            string validationMessage;
            if (!TemplateValidator.Validate(selectedTemplate, out validationMessage))
            {
                MessageBox.Show(validationMessage, "模板校验失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string templateFile = Path.Combine(App.ReportTemplatesFolder, selectedTemplate.Name);

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
                CommentColumnInsertTable = commentColumnInsertTable,
                InspectionString = InspectionComboBox.Text
                ,
                DeletePositionInBridgeDeckCheckBox = Convert.ToBoolean(appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value, CultureInfo.InvariantCulture)
                ,
                DeletePositionInSuperSpaceCheckBox = Convert.ToBoolean(appConfig.AppSettings.Settings["DeletePositionInSuperSpace"].Value, CultureInfo.InvariantCulture)
                ,
                CustomTableCellWidth = Convert.ToBoolean(appConfig.AppSettings.Settings["CustomSummaryTableWidth"].Value, CultureInfo.InvariantCulture)
                ,

                SaveDocxFormat = deserializedConfig.General.SaveDocxFormat,
                PictureTableCellWidth = ConvertUtil.MillimeterToPoint(deserializedConfig.General.PictureTableCellWidth),
                DamageDescriptionInPictureSplitSymbol = deserializedConfig.General.DamageDescriptionInPictureSplitSymbol,
                PictureNoSplitSymbol = deserializedConfig.General.PictureNoSplitSymbol,
                IntactStructNoInsertSummaryTableString = deserializedConfig.General.IntactStructNoInsertSummaryTableString,

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

            await GenerateReportAsync(generateReportSettings, templateFile, outputFile, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);
        }

        private async Task GenerateReportAsync(GenerateReportSettings generateReportSettings, string templateFile, string outputFile, ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary, ObservableCollection<DamageSummary> _superSpaceListDamageSummary, ObservableCollection<DamageSummary> _subSpaceListDamageSummary)
        {
            var w = new RegularProgressBarWindow();
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.4 * (App.ScreenWidth - w.Width);

            var progressBarModel = new ProgressBarModel
            {
                ProgressValue = 0,
                Content = "正在校验图片..."
            };
            w.DataContext = progressBarModel;
            w.progressBarNumberTextBlock.DataContext = progressBarModel;
            w.progressBar.DataContext = progressBarModel;
            w.progressBarContentTextBlock.DataContext = progressBarModel;

            List<DamageSummary> l1 = _bridgeDeckListDamageSummary.ToList();
            List<DamageSummary> l2 = _superSpaceListDamageSummary.ToList();
            List<DamageSummary> l3 = _subSpaceListDamageSummary.ToList();

            DamageSummaryServices.InitListDamageSummary1(l1, generateReportSettings.BookmarkSettings.BridgeDeckBookmarkStartNo);
            DamageSummaryServices.InitListDamageSummary1(l2, generateReportSettings.BookmarkSettings.SuperSpaceBookmarkStartNo);
            DamageSummaryServices.InitListDamageSummary1(l3, generateReportSettings.BookmarkSettings.SubSpaceBookmarkStartNo);

            var token = progressBarModel.CancellationTokenSource.Token;

            try
            {
                // 先显示进度窗口以便用户可立即点击取消
                w.Show();

                var validationResult = await PictureServices.ValidatePicturesAsync(l1, l2, l3, token);
                if (validationResult.TotalInvalidPictureCounts > 0)
                {
                    WriteInvalidPicturesResultToTxt(validationResult.TotalInvalidPictureCounts, validationResult.BridgeDeckResults, validationResult.SuperSpaceResults, validationResult.SubSpaceResults);
                    UserNotification.Warn($"存在无效照片，无法生成报告，共计{validationResult.TotalInvalidPictureCounts}张，详见根目录{App.InvalidPicturesStoreFile}");
                    return;
                }

                token.ThrowIfCancellationRequested();

                if (token.IsCancellationRequested)
                {
                    UserNotification.Info("已取消图片校验或生成。");
                    return;
                }

                progressBarModel.Content = "正在生成报告...";

                await Task.Run(() =>
                {
                    token.ThrowIfCancellationRequested();
                    Document doc = new Document(templateFile);
                    var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, l1, l2, l3);
                    asposeService.GenerateReport(ref progressBarModel);
                    token.ThrowIfCancellationRequested();

                    if (generateReportSettings.SaveDocxFormat)
                    {
                        doc.Save($"{outputFile}.docx", SaveFormat.Docx);
                    }
                    else
                    {
                        doc.Save($"{outputFile}.doc", SaveFormat.Doc);
                    }
                }, token);

                UserNotification.Info("成功生成报告！");
            }
            catch (OperationCanceledException)
            {
                UserNotification.Info("已取消图片校验或生成。");
            }
            catch (Exception ex)
            {
                UserNotification.Error("生成报告时发生异常，请查看日志。", ex);
            }
            finally
            {
                w.Dispatcher.BeginInvoke(new Action(() => w.Close()));
            }
        }

        /// <summary>
        /// 生成报告的方法，使用Dictionary<string, List<DamageSummary>>作为参数。
        /// </summary>
        /// <param name="generateReportSettings">报告生成设置。</param>
        /// <param name="templateFile">模板文件路径。</param>
        /// <param name="outputFile">输出文件路径。</param>
        /// <param name="damageSummaries">包含工作表名称和损坏数据的字典。</param>
        public static void GenerateReport(GenerateReportSettings generateReportSettings, string templateFile, string outputFile, Dictionary<string, List<DamageSummary>> damageSummaries)
        {
            if (damageSummaries == null || damageSummaries.Count < 1)
            {
                throw new ArgumentException("必须至少提供一个DamageSummary列表。");
            }

            var w = new RegularProgressBarWindow();
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

            //通用版本直接传入List，不用转换
            //List<DamageSummary> l1 = _bridgeDeckListDamageSummary.ToList();
            //List<DamageSummary> l2 = _superSpaceListDamageSummary.ToList();
            //List<DamageSummary> l3 = _subSpaceListDamageSummary.ToList();

            //传入后已经初始化，不需要再初始化
            //DamageSummaryServices.InitListDamageSummary1(l1, generateReportSettings.BookmarkSettings.BridgeDeckBookmarkStartNo);
            //DamageSummaryServices.InitListDamageSummary1(l2, generateReportSettings.BookmarkSettings.SuperSpaceBookmarkStartNo);
            //DamageSummaryServices.InitListDamageSummary1(l3, generateReportSettings.BookmarkSettings.SubSpaceBookmarkStartNo);

            var thread = new Thread(new ThreadStart(() =>
            {
                //progressBarModel.ProgressValue = 0;    //测试数据
                //生成报告前先验证照片的有效性
 
                int totalInvalidPictureCounts = PictureServices.ValidatePictures(damageSummaries, out Dictionary<string, List<string>> validationResults);

                if (totalInvalidPictureCounts > 0)
                {
                    try
                    {
                        WriteInvalidPicturesResultToTxt(totalInvalidPictureCounts, validationResults);
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
                var asposeService = new AsposeWordsServices(ref doc, generateReportSettings, damageSummaries);
                asposeService.GenerateReport(ref progressBarModel);

                if (generateReportSettings.SaveDocxFormat)
                {
                    doc.Save($"{outputFile}.docx", SaveFormat.Docx);
                }
                else
                {
                    doc.Save($"{outputFile}.doc", SaveFormat.Doc);
                }

                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("成功生成报告！"); });

            }));
            thread.Start();

            foreach (var kvp in damageSummaries)
            {
                string worksheetName = kvp.Key;
                List<DamageSummary> damageSummaryList = kvp.Value;

                // 对每个DamageSummary列表进行处理
                // 示例处理代码：
                Console.WriteLine($"Processing worksheet: {worksheetName}");
                foreach (var damageSummary in damageSummaryList)
                {
                    // 处理每个DamageSummary对象
                }
            }
        }
    }
}
