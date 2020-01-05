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
            var ds = new DamageSummaryServices();

            List<DamageSummary> lst;

            lst = dataRepository.ReadDamageData(BridgePart.BridgeDeck);
            ds.InitListDamageSummary(lst);
            BridgeDeckGrid.ItemsSource = lst;

            lst = dataRepository.ReadDamageData(BridgePart.SuperSpace);
            ds.InitListDamageSummary(lst, 2_000_000);
            SuperSpaceGrid.ItemsSource = lst;

            lst = dataRepository.ReadDamageData(BridgePart.SubSpace);
            ds.InitListDamageSummary(lst, 3_000_000);
            SubSpaceGrid.ItemsSource = lst;

        }


        private void AutoReport_Click(object sender, RoutedEventArgs e)
        {
            string templateFile = "外观检查报告模板.docx"; string outputFile = "自动生成的外观检查报告.docx";
            double ImageWidth = 224.25; double ImageHeight = 168.75;
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

                        var asposeService = new AsposeWordsServices(ref doc, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);

                        asposeService.GenerateSummaryTableAndPictureTable(ImageWidth, ImageHeight, CompressImageFlag);

                        doc.UpdateFields();
                        doc.UpdateFields();

                        doc.Save(outputFile, SaveFormat.Docx);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }));
            }).Start();


        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_Option_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        private void MenuItem_ViewSourceCode_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/zjkl19/AutoRegularInspection/");
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            //TODO：通过反射读取 AssemblyCopyright
            MessageBox.Show($"当前版本v{Application.ResourceAssembly.GetName().Version.ToString()}\r" +
            $"Copyright © 福建省建筑科学研究院 福建省建筑工程质量检测中心有限公司 2020\r" +
            $"系统框架设计、编程及维护：路桥检测研究所林迪南等"
            , "关于");
        }

        private void DisclaimerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }
        private void InstructionsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }

        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("该功能开发中");
        }


    }
}
