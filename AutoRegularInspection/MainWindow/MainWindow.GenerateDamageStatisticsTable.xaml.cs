using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void GenerateDamageStatisticsTable_Click(object sender, RoutedEventArgs e)
        {

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;

            try
            {
                DamageSummaryServices.GenerateDamageStatisticsTable(_bridgeDeckListDamageSummary);
                MessageBox.Show("成功生成桥梁检测病害统计汇总表！");
            }
            catch (Exception ex)
            {
                Debug.Print($"保存excel出错，错误信息：{ex.Message}");
            }

        }

    }
}
