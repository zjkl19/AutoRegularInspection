﻿using Aspose.Words;
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

namespace AutoRegularInspection
{

    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();
        public ILogger log;
        public MainWindow()
        {

            InitializeComponent();

            Title = $"外观检查自动报告 v{Application.ResourceAssembly.GetName().Version.ToString()}";

            //Nlog
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = @"Log\LogFile.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            log = LogManager.GetCurrentClassLogger();

            
            //TODO:考虑放到App.xaml中
            //IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            //var dataRepository = kernel.Get<IDataRepository>();

            BridgeDeckGrid.DataContext = new GridViewModel();

            SuperSpaceGrid.DataContext = new GridViewModel(BridgePart.SuperSpace);

            SubSpaceGrid.DataContext = new GridViewModel(BridgePart.SubSpace);

            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool commentColumnInsertTable;
            commentColumnInsertTable = Convert.ToBoolean(appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value);

            if (commentColumnInsertTable)
            {
                CommentColumnInsertTableCheckBox.IsChecked = true;
            }
            else
            {
                CommentColumnInsertTableCheckBox.IsChecked = false;
            }

            CheckForUpdateInStarup();    //启动时检查更新
        }

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
                    GenerateReport(commentColumnInsertTable,ImageWidth, ImageHeight, templateFile, outputFile, CompressImageFlag, _bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);

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

        private static void GenerateReport(bool CommentColumnInsertTable,double ImageWidth, double ImageHeight, string templateFile, string outputFile, int CompressImageFlag, ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary, ObservableCollection<DamageSummary> _superSpaceListDamageSummary, ObservableCollection<DamageSummary> _subSpaceListDamageSummary)
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

        private void MenuItem_Option_Click(object sender, RoutedEventArgs e)
        {
            var w = new OptionWindow();
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.5 * (App.ScreenWidth - w.Width);
            w.Show();
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
                    MessageBox.Show($"成功备份文件\"{Path.GetFileNameWithoutExtension(App.DamageSummaryFileName)} - 副本 ({i}).xlsx\"");
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
            if (File.Exists(App.DamageSummaryFileName))
            {
                Process.Start(App.DamageSummaryFileName);
            }
            else
            {
                MessageBox.Show($"未找到文件{App.DamageSummaryFileName}");
            }

        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            string reportFile = App.OutputReportFileName;
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

        private void AutoCheckForUpdateCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
