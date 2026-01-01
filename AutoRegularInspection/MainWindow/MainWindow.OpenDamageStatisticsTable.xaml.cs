using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoRegularInspection.Services;

namespace AutoRegularInspection
{
    public partial class MainWindow
    {
        private void OpenDamageStatisticsTable_Click(object sender, RoutedEventArgs e)
        {
            string reportFile = App.OutputDamageStatisticsFileName;
            if (File.Exists(reportFile))
            {
                Process.Start(reportFile);
            }
            else
            {
                UserNotification.Warn("请先生成报告。");
            }
        }
    }
}
