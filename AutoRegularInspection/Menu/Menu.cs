using AutoRegularInspection.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void MenuItem_Option_Click(object sender, RoutedEventArgs e)
        {
            OptionWindow w = new OptionWindow(_log);
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
            _ = MessageBox.Show($"当前版本v{Application.ResourceAssembly.GetName().Version}\r" +
            $"Copyright © 福建省建筑科学研究院 福建省建筑工程质量检测中心有限公司 2020-2023\r" +
            "系统框架设计、编程及维护：路桥检测研究所林迪南等"
            , "关于");
        }
    }
}

