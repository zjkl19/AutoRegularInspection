using AutoRegularInspection.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 复制选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyRow_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = BridgeDeckGrid;
            if (BridgeDeckTabItem.IsSelected)
            {
                dg = BridgeDeckGrid;
            }
            else if (SuperSpaceTabItem.IsSelected)
            {
                dg = SuperSpaceGrid;
            }
            else if (SubSpaceTabItem.IsSelected)
            {
                dg = SubSpaceGrid;
            }
            int selectedIndex = dg.SelectedIndex;
            if (selectedIndex >= 0)
            {
                ObservableCollection<DamageSummary> listDamageSummary = dg.ItemsSource as ObservableCollection<DamageSummary>;
                listDamageSummary.Add(listDamageSummary[selectedIndex]);
            }


        }
    }
}
