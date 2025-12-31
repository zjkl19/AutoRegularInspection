
using System.Windows;
using System;
using System.IO;
using System.Diagnostics;
using System.Text; // 引入文本命名空间以访问编码类
using Aspose.Words;
using Aspose.Words.Tables;
using AutoRegularInspection.Models;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoRegularInspection.IRepository;
using AutoRegularInspection.Services;
using System.Collections.Generic;
using Ninject;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
       
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool commentColumnInsertTable;
            commentColumnInsertTable = Convert.ToBoolean(appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value, CultureInfo.InvariantCulture);

            XDocument config = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");

            //反序列化XML配置文件
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

            //默认ListDamageSummary是从UI读取，通用版本直接从excel读取
            //var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            //var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            //var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;

            //通用版本，参考GridViewModel代码
            List<DamageSummary> lst;
            IKernel kernel = App.Kernel ?? new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            // 读取Excel文件中所有标签的数据
            Dictionary<string, List<DamageSummary>> allDamageData = dataRepository.ReadAllDamageDataFromFile("外观检查-通用.xlsx");
            int initialIndex = 1000000;    // 初始化初始索引值
            // 遍历Dictionary并对每个List<DamageSummary>进行预处理
            foreach (var kvp in allDamageData)
            {
                string worksheetName = kvp.Key;
                List<DamageSummary> damageSummaryList = kvp.Value;
                // 对每个List<DamageSummary>调用InitListDamageSummary进行预处理
                DamageSummaryServices.InitListDamageSummary(damageSummaryList, initialIndex, BridgePart.SuperSpace); // 根据需要传递不同的参数
                initialIndex += 100000;    // 递增初始索引值
            }

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

            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    GenerateReport(generateReportSettings, templateFile, outputFile, allDamageData);
                }));
            }).Start();


        }


    }
}
