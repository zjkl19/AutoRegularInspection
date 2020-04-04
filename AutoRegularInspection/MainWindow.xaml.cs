using Aspose.Words;

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
using AutoRegularInspection.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            //IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            //var dataRepository = kernel.Get<IDataRepository>();

            BridgeDeckGrid.DataContext = new GridViewModel();

            SuperSpaceGrid.DataContext = new GridViewModel(BridgePart.SuperSpace);

            SubSpaceGrid.DataContext = new GridViewModel(BridgePart.SubSpace);
            //TODO：Grid数据和Excel绑定
            //List<DamageSummary> lst;




            //lst = dataRepository.ReadDamageData(BridgePart.SuperSpace);

            //生成报告的时候再调用该代码
            //DamageSummaryServices.InitListDamageSummary(lst, 2_000_000,BridgePart.SuperSpace);
            //SuperSpaceGrid.ItemsSource = lst;

            //lst = dataRepository.ReadDamageData(BridgePart.SubSpace);
            //生成报告的时候再调用该代码
            //DamageSummaryServices.InitListDamageSummary(lst, 3_000_000, BridgePart.SubSpace);
            //SubSpaceGrid.ItemsSource = lst;

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

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;



            new Thread(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        GenerateReport(ImageWidth, ImageHeight, templateFile, outputFile, CompressImageFlag, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }));
            }).Start();


        }

        private static void GenerateReport(double ImageWidth, double ImageHeight, string templateFile, string outputFile, int CompressImageFlag, ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary, ObservableCollection<DamageSummary> _superSpaceListDamageSummary, ObservableCollection<DamageSummary> _subSpaceListDamageSummary)
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

            //List<DamageSummary> lst1 = _bridgeDeckListDamageSummary.ToList();
            //List<DamageSummary> lst2 = _superSpaceListDamageSummary.ToList();
            //List<DamageSummary> lst3 = _subSpaceListDamageSummary.ToList();

            //List<DamageSummary> l1 = new List<DamageSummary>();
            //List<DamageSummary> l2 = new List<DamageSummary>();
            //List<DamageSummary> l3 = new List<DamageSummary>();

            //lst1.ForEach(i => l1.Add(i));
            //lst2.ForEach(i => l3.Add(i));
            //lst2.ForEach(i => l3.Add(i));



            DamageSummaryServices.InitListDamageSummary1(l1);
            DamageSummaryServices.InitListDamageSummary1(l2, 2_000_000);
            DamageSummaryServices.InitListDamageSummary1(l3, 3_000_000);
            


            var thread = new Thread(new ThreadStart(() =>
            {
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Show(); });
                progressBarModel.ProgressValue = 10;


                var doc = new Document(templateFile);
                var asposeService = new AsposeWordsServices(ref doc, l1, l2, l3);
                asposeService.GenerateSummaryTableAndPictureTable(ref progressBarModel, ImageWidth, ImageHeight, CompressImageFlag);

                doc.UpdateFields();
                doc.UpdateFields();

                doc.Save(outputFile, SaveFormat.Docx);
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { w.Close(); });
                w.progressBar.Dispatcher.BeginInvoke((ThreadStart)delegate { MessageBox.Show("成功生成报告！"); });

            }));
            thread.Start();

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
                var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
                var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
                var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
                if (SaveExcelService.SaveExcel(_bridgeDeckListDamageSummary.ToList()
                    , _superSpaceListDamageSummary.ToList()
                    , _subSpaceListDamageSummary.ToList()) == 1)
                {
                    MessageBox.Show("Excel保存成功！");
                }
                else
                {
                    MessageBox.Show("Excel保存失败！");
                }

            }

        }

        private void OpenExcel_Click(object sender, RoutedEventArgs e)
        {
            string xlsxFile = "外观检查.xlsx";
            if (File.Exists(xlsxFile))
            {
                Process.Start(xlsxFile);
            }
            else
            {
                MessageBox.Show($"未找到文件{xlsxFile}");
            }

        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            string reportFile = "自动生成的外观检查报告.docx";
            if (File.Exists(reportFile))
            {
                Process.Start(reportFile);
            }
            else
            {
                MessageBox.Show($"请先生成报告。");
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
            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = BridgeDeckGrid;
            ChangeDamageComboBox(dataGrid, BridgePart.BridgeDeck);

        }

        private static void ChangeDamageComboBox(DataGrid dataGrid, BridgePart bridgePart)
        {
            int rowIndex = 0;
            var _cells = dataGrid.SelectedCells;//获取选中单元格的列表
            if (_cells.Any())
            {

                rowIndex = dataGrid.Items.IndexOf(_cells.First().Item);
                //columnIndex = _cells.First().Column.DisplayIndex;

            }
            var _bridgeDeckListDamageSummary = dataGrid.ItemsSource as ObservableCollection<DamageSummary>;

            IEnumerable<BridgeDamage> componentFound;
            if (bridgePart == BridgePart.BridgeDeck)
            {
                componentFound = GlobalData.ComponentComboBox.Where(x => x.Title == _bridgeDeckListDamageSummary[rowIndex].Component);
            }
            else if (bridgePart == BridgePart.SuperSpace)
            {
                componentFound = GlobalData.SuperSpaceComponentComboBox.Where(x => x.Title == _bridgeDeckListDamageSummary[rowIndex].Component);
            }
            else
            {
                componentFound = GlobalData.SubSpaceComponentComboBox.Where(x => x.Title == _bridgeDeckListDamageSummary[rowIndex].Component);
            }

            if (componentFound.Any())
            {

                var subComponentFound = componentFound.FirstOrDefault().DamageComboBox.Where(x => x.Title == _bridgeDeckListDamageSummary[rowIndex].Damage);

                _bridgeDeckListDamageSummary[rowIndex].DamageComboBox = componentFound.FirstOrDefault().DamageComboBox;

                _bridgeDeckListDamageSummary[rowIndex].DamageValue = componentFound.FirstOrDefault().DamageComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx;

            }
            else
            {

                _bridgeDeckListDamageSummary[rowIndex].DamageComboBox = GlobalData.ComponentComboBox.Where(x => x.Title == "其它").FirstOrDefault().DamageComboBox;
                _bridgeDeckListDamageSummary[rowIndex].ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx;
            }
        }


        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = BridgeDeckGrid;

            UIChangeDamageComboBox(curComboBox, dataGrid, GlobalData.ComponentComboBox);

        }
        private void SuperSpaceComboBox_DropDownClosed(object sender, EventArgs e)
        {

            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = SuperSpaceGrid;

            UIChangeDamageComboBox(curComboBox, dataGrid, GlobalData.SuperSpaceComponentComboBox);
        }
        private void SubSpaceComboBox_DropDownClosed(object sender, EventArgs e)
        {

            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = SubSpaceGrid;

            UIChangeDamageComboBox(curComboBox, dataGrid, GlobalData.SubSpaceComponentComboBox);
        }

        private static void UIChangeDamageComboBox(ComboBox curComboBox, DataGrid dataGrid, BindingList<BridgeDamage> componentComboBox)
        {
            int rowIndex = 0;
            var _cells = dataGrid.SelectedCells;//获取选中单元格的列表
            if (_cells.Any())
            {

                rowIndex = dataGrid.Items.IndexOf(_cells.First().Item);
                //columnIndex = _cells.First().Column.DisplayIndex;

            }
            var _bridgeDeckListDamageSummary = dataGrid.ItemsSource as ObservableCollection<DamageSummary>;

            if (rowIndex >= _bridgeDeckListDamageSummary.Count)
            {
                return;
            }

            var m = curComboBox.SelectedIndex;
            BridgeDamage componentFoundBefore = null;


            componentFoundBefore = componentComboBox[_bridgeDeckListDamageSummary[rowIndex].ComponentValue];

            var componentFound = componentComboBox[curComboBox.SelectedIndex];


            if (componentFound.Title != componentFoundBefore.Title)
            {
                _bridgeDeckListDamageSummary[rowIndex].DamageComboBox = componentFound.DamageComboBox;

                _bridgeDeckListDamageSummary[rowIndex].DamageValue = 0;

            }
        }



        private void DamageComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = BridgeDeckGrid;

            ChangeDamageValue(curComboBox, dataGrid);

        }

        private void SuperSpaceDamageComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = SuperSpaceGrid;

            ChangeDamageValue(curComboBox, dataGrid);

        }

        private void SubSpaceDamageComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox curComboBox = sender as ComboBox;

            DataGrid dataGrid = SubSpaceGrid;

            ChangeDamageValue(curComboBox, dataGrid);

        }
        private static void ChangeDamageValue(ComboBox curComboBox, DataGrid dataGrid)
        {
            int rowIndex = 0;
            var _cells = dataGrid.SelectedCells;//获取选中单元格的列表
            if (_cells.Any())
            {

                rowIndex = dataGrid.Items.IndexOf(_cells.First().Item);
                //columnIndex = _cells.First().Column.DisplayIndex;

            }
            var _bridgeDeckListDamageSummary = dataGrid.ItemsSource as ObservableCollection<DamageSummary>;

            if (rowIndex >= _bridgeDeckListDamageSummary.Count)
            {
                MessageBox.Show("请先双击序号一列以初始化改行");
                return;
            }

            var m = curComboBox.SelectedIndex;
            //var componentFoundBefore = GlobalData.ComponentComboBox[_bridgeDeckListDamageSummary[rowIndex].ComponentValue];
            //var componentFound = GlobalData.ComponentComboBox[curComboBox.SelectedIndex];


            _bridgeDeckListDamageSummary[rowIndex].DamageValue = curComboBox.SelectedIndex;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            //_bridgeDeckListDamageSummary[0].DamageDescription = "lbt";

            //_bridgeDeckListDamageSummary[2].DamageComboBox = GlobalData.ComponentComboBox[5].DamageComboBox;
            //_bridgeDeckListDamageSummary[2].DamageValue = 2;
            //MessageBox.Show(_bridgeDeckListDamageSummary[2].Component);

            try
            {

                MessageBox.Show(_bridgeDeckListDamageSummary[5].ComponentValue.ToString());
                MessageBox.Show(_bridgeDeckListDamageSummary[5].DamageValue.ToString());
                MessageBox.Show(_bridgeDeckListDamageSummary[5].Component.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DamageComboBox_DropDownOpened(object sender, EventArgs e)
        {

            DataGrid dataGrid = BridgeDeckGrid;

            SetDamageDropDownItems(dataGrid);

        }

        private void SuperSpaceDamageComboBox_DropDownOpened(object sender, EventArgs e)
        {

            DataGrid dataGrid = SuperSpaceGrid;

            SetDamageDropDownItems(dataGrid);

        }

        private void SubSpaceDamageComboBox_DropDownOpened(object sender, EventArgs e)
        {

            DataGrid dataGrid = SubSpaceGrid;

            SetDamageDropDownItems(dataGrid);

        }

        private static void SetDamageDropDownItems(DataGrid dataGrid)
        {
            int rowIndex = 0;
            var _cells = dataGrid.SelectedCells;//获取选中单元格的列表
            if (_cells.Any())
            {

                rowIndex = dataGrid.Items.IndexOf(_cells.First().Item);
            }
            var _bridgeDeckListDamageSummary = dataGrid.ItemsSource as ObservableCollection<DamageSummary>;


            if (_bridgeDeckListDamageSummary[rowIndex].DamageComboBox == null)
            {
                _bridgeDeckListDamageSummary[rowIndex].DamageComboBox = GlobalData.ComponentComboBox[_bridgeDeckListDamageSummary[rowIndex].ComponentValue].DamageComboBox;
            }
        }
    }
}
