using AutoRegularInspection.Services;
using AutoRegularInspection.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoRegularInspection.Services;


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

        private void MenuItem_ReloadConfig_Click(object sender, RoutedEventArgs e)
        {
            ReloadConfigAndTemplates();
        }

        private void StatusBarReloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadConfigAndTemplates();
        }

        private void ReloadConfigAndTemplates()
        {
            try
            {
                App.ReloadTemplates();
                OptionConfigurationLoader.Load(null, true);

                TemplateFileComboBox.ItemsSource = App.TemplateFileList;
                if (App.TemplateFileList != null && App.TemplateFileList.Count > 0)
                {
                    TemplateFileComboBox.SelectedIndex = 0;
                }

                UpdateStatusBar();
                UserNotification.Info("配置与模板已重新加载。");
            }
            catch (Exception ex)
            {
                UserNotification.Error($"重新加载配置或模板失败：{ex.Message}", ex);
            }
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            var copyrightAttribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
    Assembly.GetExecutingAssembly(),
    typeof(AssemblyCopyrightAttribute));
            string copyright = copyrightAttribute != null ? copyrightAttribute.Copyright : "版权所有";

            _ = MessageBox.Show($"当前版本v{Application.ResourceAssembly.GetName().Version}\r" +
            $"{copyright}\r" +
            "系统框架设计、编程及维护：桥梁监测与数字化研究所林迪南，等"
            , "关于");
        }
    }
}

