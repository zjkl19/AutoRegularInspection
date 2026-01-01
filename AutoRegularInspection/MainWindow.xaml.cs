using Aspose.Words;
using AutoRegularInspection.Models;
using System.Collections.Generic;

using System.IO;
using System.Windows;
using System.Linq;
using AutoRegularInspection.Services;

using System;
using System.Threading;

using AutoRegularInspection.Views;
using System.Xml.Linq;
using System.Diagnostics;

using System.Windows.Controls;
using System.Windows.Threading;
using AutoRegularInspection.ViewModels;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Globalization;
using System.Xml.Serialization;
using System.Windows.Media;
using System.Diagnostics;

namespace AutoRegularInspection
{

    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();
        private readonly ILogger _log;
        public MainWindow()
        {

            InitializeComponent();

            Title = $"外观检查自动报告 v{Application.ResourceAssembly.GetName().Version}";

            //Nlog
            LoggingConfiguration config = new LoggingConfiguration();

            // Targets where to log to: File and Console
            FileTarget logfile = new FileTarget("logfile") { FileName = @"Log\LogFile.txt" };
            ConsoleTarget logconsole = new ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            _log = LogManager.GetCurrentClassLogger();

            //TODO:考虑放到App.xaml中
            //IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            //var dataRepository = kernel.Get<IDataRepository>();

            //初始化ComboBoxReportTemplates
            TemplateFileComboBox.ItemsSource = App.TemplateFileList;
            TemplateFileComboBox.SelectedIndex = 0;
            TemplateFileComboBox.SelectionChanged += TemplateFileComboBox_SelectionChanged;
            UpdateStatusBar();

            BridgeDeckGrid.DataContext = new GridViewModel();
            SuperSpaceGrid.DataContext = new GridViewModel(BridgePart.SuperSpace);
            SubSpaceGrid.DataContext = new GridViewModel(BridgePart.SubSpace);

            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool commentColumnInsertTable;
            commentColumnInsertTable = Convert.ToBoolean(appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value, CultureInfo.InvariantCulture);

            CommentColumnInsertTableCheckBox.IsChecked = commentColumnInsertTable;

            bool deletePositionInBridgeDeck = Convert.ToBoolean(appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value, CultureInfo.InvariantCulture);

            DeletePositionInBridgeDeckCheckBox.IsChecked = deletePositionInBridgeDeck;

            bool customSummaryTableWidth = Convert.ToBoolean(appConfig.AppSettings.Settings["CustomSummaryTableWidth"].Value,CultureInfo.InvariantCulture);

            CustomSummaryTableWidthCheckBox.IsChecked = customSummaryTableWidth;

            CheckForUpdateInStarup();    //启动时检查更新
        }


        /// <summary>
        /// 备份excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///<remarks>
        ///算法：检测是否存在"外观检查 - 副本 (1).xlsx"，若不存在，则复制保存。
        ///若存在，检测是否存在"外观检查 - 副本 (2).xlsx",若不存在，则复制保存，以此类推。
        ///</remarks>
        private void BackupExcel_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            try
            {
                while (File.Exists($"{Path.GetFileNameWithoutExtension(App.DamageSummaryFileName)} - 副本 ({i}).xlsx"))
                {
                    i++;
                }
                if (File.Exists(App.DamageSummaryFileName))
                {
                    File.Copy(App.DamageSummaryFileName, $"{Path.GetFileNameWithoutExtension(App.DamageSummaryFileName)} - 副本 ({i}).xlsx", true);
                    UserNotification.Info($"成功备份文件\"{Path.GetFileNameWithoutExtension(App.DamageSummaryFileName)} - 副本 ({i}).xlsx\"");
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"备份Excel表格出错，错误信息：{ex.Message}");
                _log.Error($"{nameof(BackupExcel_Click)}:{ex.Message}");
            }

        }

        private void SaveExcel_Click(object sender, RoutedEventArgs e)
        {
            if (UserNotification.Confirm("保存后将会覆盖原来的Excel文件，你确定要继续吗？", "保存Excel"))
            {
                var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
                var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
                var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;

                if (SaveExcelService.SaveExcel(_bridgeDeckListDamageSummary.ToList()
                    , _superSpaceListDamageSummary.ToList()
                    , _subSpaceListDamageSummary.ToList()) == 1)
                {
                    UserNotification.Info("Excel保存成功！");
                }
                else
                {
                    UserNotification.Error("Excel保存失败！");
                }
            }
        }

        private void OpenExcel_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(App.DamageSummaryFileName))
            {
                Process.Start(App.DamageSummaryFileName);
            }
            else
            {
                UserNotification.Warn($"未找到文件{App.DamageSummaryFileName}");
            }

        }

        private void TemplateFileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatusBar();
        }

        private void OpenTemplateFolderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(App.ReportTemplatesFolder);
            }
            catch (Exception ex)
            {
                UserNotification.Error("无法打开模板目录，请手动检查路径。", ex);
            }
        }

        private void UpdateStatusBar()
        {
            if (TemplateStatusTextBlock != null)
            {
                if (TemplateFileComboBox.SelectedIndex >= 0 && App.TemplateFileList != null && TemplateFileComboBox.SelectedIndex < App.TemplateFileList.Count)
                {
                    var selected = App.TemplateFileList[TemplateFileComboBox.SelectedIndex];
                    string msg;
                    var ok = TemplateValidator.Validate(selected, out msg);
                    TemplateStatusTextBlock.Text = ok ? $"模板状态：可用（{selected.DisplayName}）" : $"模板状态：不可用（{msg}）";
                    TemplateStatusTextBlock.Foreground = ok ? Brushes.ForestGreen : Brushes.Firebrick;
                }
                else
                {
                    TemplateStatusTextBlock.Text = "模板状态：未选择";
                    TemplateStatusTextBlock.Foreground = Brushes.Black;
                }
            }

            if (ConfigPathTextBlock != null)
            {
                ConfigPathTextBlock.Text = $"配置路径：{Path.Combine(App.ConfigurationFolder, App.ConfigFileName)}";
            }

            if (LastReloadTextBlock != null)
            {
                LastReloadTextBlock.Text = $"上次加载：{App.TemplatesLastLoadedAt}";
            }
        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            XDocument config = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");

            //反序列化XML配置文件
            var deserializedConfig = OptionConfigurationLoader.Load();
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

            string reportFile = App.OutputReportFileName;
            string fileExtension = generateReportSettings.SaveDocxFormat ? ".docx" : ".doc";
            string fullPath = $"{reportFile}{fileExtension}";

            if (File.Exists(fullPath))
            {
                Process.Start(fullPath);
            }
            else
            {
                UserNotification.Warn("请先生成报告。");
            }

        }

        private void DisclaimerButton_Click(object sender, RoutedEventArgs e)
        {
            UserNotification.Warn("本软件计算结果及生成的报告等仅供参考，因本软件产生的计算错误、生成报告结果不正确的后果由软件使用者自行承担。");
        }
        private void InstructionsButton_Click(object sender, RoutedEventArgs e)
        {
            UserNotification.Info("该功能开发中");
        }

        private void AutoCheckForUpdateCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (AutoCheckForUpdateCheckBox.IsChecked ?? false)
                {
                    appConfig.AppSettings.Settings["AutoCheckForUpdate"].Value = "true";
                }
                else
                {
                    appConfig.AppSettings.Settings["AutoCheckForUpdate"].Value = "false";
                }

                appConfig.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                _log.Error($"{nameof(AutoCheckForUpdateCheckBox_Click)}:{ex.Message}");
            }

        }
    }
}
