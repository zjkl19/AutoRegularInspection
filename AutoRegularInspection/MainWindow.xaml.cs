﻿using Aspose.Words;

using AutoRegularInspection.Models;
using System.Collections.Generic;

using System.IO;
using System.Windows;
using System.Linq;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Threading;

using Ninject;
using AutoRegularInspection.IRepository;
using AutoRegularInspection.Views;
using System.Xml.Linq;
using System.Diagnostics;

#region DataGridSingleClick
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Threading;
#endregion
namespace AutoRegularInspection
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            //TODO:考虑放到App.xaml中
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            //TODO：Grid数据和Excel绑定
            List<DamageSummary> lst;

            lst = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            DamageSummaryServices.InitListDamageSummary(lst);
            BridgeDeckGrid.ItemsSource = lst;

            lst = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            DamageSummaryServices.InitListDamageSummary(lst, 2_000_000);
            SuperSpaceGrid.ItemsSource = lst;

            lst = dataRepository.ReadDamageData(BridgePart.SubSpace);
            DamageSummaryServices.InitListDamageSummary(lst, 3_000_000);
            SubSpaceGrid.ItemsSource = lst;

        }

        #region DataGridSingleClickCell
        //private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DataGridCell cell = sender as DataGridCell;
        //    GridColumnFastEdit(cell, e);
        //}

        //private void DataGridCell_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    DataGridCell cell = sender as DataGridCell;
        //    GridColumnFastEdit(cell, e);
        //}

        //private static void GridColumnFastEdit(DataGridCell cell, RoutedEventArgs e)
        //{
        //    if (cell == null || cell.IsEditing || cell.IsReadOnly)
        //        return;

        //    DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
        //    if (dataGrid == null)
        //        return;

        //    if (!cell.IsFocused)
        //    {
        //        cell.Focus();
        //    }

        //    if (cell.Content is CheckBox)
        //    {
        //        if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
        //        {
        //            if (!cell.IsSelected)
        //                cell.IsSelected = true;
        //        }
        //        else
        //        {
        //            DataGridRow row = FindVisualParent<DataGridRow>(cell);
        //            if (row != null && !row.IsSelected)
        //            {
        //                row.IsSelected = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ComboBox cb = cell.Content as ComboBox;
        //        if (cb != null)
        //        {
        //            //DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
        //            dataGrid.BeginEdit(e);
        //            cell.Dispatcher.Invoke(
        //             DispatcherPriority.Background,
        //             new Action(delegate { }));
        //            cb.IsDropDownOpen = true;
        //        }
        //    }
        //}


        //private static T FindVisualParent<T>(UIElement element) where T : UIElement
        //{
        //    UIElement parent = element;
        //    while (parent != null)
        //    {
        //        T correctlyTyped = parent as T;
        //        if (correctlyTyped != null)
        //        {
        //            return correctlyTyped;
        //        }

        //        parent = VisualTreeHelper.GetParent(parent) as UIElement;
        //    }
        //    return null;
        //}
        #endregion
        private void AutoReport_Click(object sender, RoutedEventArgs e)
        {
 
            var config = XDocument.Load(@"Option.config");
            var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();

            //double ImageWidth = 224.25; double ImageHeight = 168.75;
            double ImageWidth = Convert.ToDouble(pictureWidth.Value.ToString()); double ImageHeight = Convert.ToDouble(pictureHeight.Value.ToString());

            string templateFile = "外观检查报告模板.docx"; string outputFile = "自动生成的外观检查报告.docx";

            int CompressImageFlag = 80;    //图片压缩质量（0-100,值越大质量越高）

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as List<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as List<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as List<DamageSummary>;
            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {

                        var doc = new Document(templateFile);

                        var asposeService = new AsposeWordsServices(ref doc, ref _bridgeDeckListDamageSummary, ref _superSpaceListDamageSummary, ref _subSpaceListDamageSummary);
                        asposeService.GenerateSummaryTableAndPictureTable(ImageWidth, ImageHeight, CompressImageFlag);

                        doc.UpdateFields();
                        doc.UpdateFields();

                        doc.Save(outputFile, SaveFormat.Docx);
                        MessageBox.Show("成功生成报告！");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }));
            }).Start();

        }

        private void MenuItem_Option_Click(object sender, RoutedEventArgs e)
        {
            var w = new OptionWindow();
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.5 * (App.ScreenWidth - w.Width);
            w.Show();
        }

        private void SuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new SuggestionWindow();
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.5 * (App.ScreenWidth - w.Width);
            w.Show();

            var s = new SuggestionServices();

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as List<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as List<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as List<DamageSummary>;

            var lst = _bridgeDeckListDamageSummary.Union(_superSpaceListDamageSummary).Union(_subSpaceListDamageSummary).ToList<DamageSummary>();

            w.SuggestionTextBox.Text = s.MakeSuggestions(lst);

        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_ViewSourceCode_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/zjkl19/AutoRegularInspection/");
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            //TODO：通过反射读取 AssemblyCopyright
            MessageBox.Show($"当前版本v{Application.ResourceAssembly.GetName().Version.ToString()}\r" +
            $"Copyright © 福建省建筑科学研究院 福建省建筑工程质量检测中心有限公司 2020\r" +
            $"系统框架设计、编程及维护：路桥检测研究所林迪南等"
            , "关于");
        }

        //算法：检测是否存在"外观检查 - 副本 (1).xlsx"，若不存在，则复制保存
        //若存在，检测是否存在"外观检查 - 副本 (2).xlsx",若不存在，则复制保存，以此类推

        private void BackupExcel_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            try
            {

                while (File.Exists($"外观检查 - 副本 ({i}).xlsx"))
                {
                    i++;
                }
                if (File.Exists($"外观检查.xlsx"))
                {
                    File.Copy($"外观检查.xlsx", $"外观检查 - 副本 ({i}).xlsx", true);
                    MessageBox.Show($"成功备份文件\"外观检查 - 副本 ({i}).xlsx\"");
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"备份Excel表格出错，错误信息：{ex.Message}");
            }
            
        }

        private void SaveExcel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("保存后将会覆盖原来的Excel文件，你确定要继续吗？", "保存Excel", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as List<DamageSummary>;
                var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as List<DamageSummary>;
                var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as List<DamageSummary>;
                if (SaveExcelService.SaveExcel(_bridgeDeckListDamageSummary
                    , _superSpaceListDamageSummary
                    , _subSpaceListDamageSummary) ==1)
                {
                    MessageBox.Show("Excel保存成功！");
                }
                else
                {
                    MessageBox.Show("Excel保存失败！");
                }
                
            }

        }
        private void DisclaimerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("本软件计算结果及生成的报告等仅供参考，因本软件产生的计算错误、生成报告结果不正确的后果由软件使用者自行承担。");
        }
        private void InstructionsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var cb = (sender as ComboBox);
            if (cb == null || cb.Tag == null) return;
            int idx = int.Parse(cb.Tag.ToString());
          

        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as List<DamageSummary>;
            //_bridgeDeckListDamageSummary[0].DamageDescription = "lbt";


           _bridgeDeckListDamageSummary[0].ComponentValue = 1;

        }
    }
}
