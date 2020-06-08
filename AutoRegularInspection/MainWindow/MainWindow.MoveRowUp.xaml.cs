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
        /// 上移选中行一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveRowUp_Click(object sender, RoutedEventArgs e)
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
            DamageSummary temp;
            int selectedIndex = dg.SelectedIndex;
            if (selectedIndex >= 1)
            {
                ObservableCollection<DamageSummary> listDamageSummary = dg.ItemsSource as ObservableCollection<DamageSummary>;
                temp = listDamageSummary[selectedIndex - 1];
                listDamageSummary[selectedIndex - 1] = listDamageSummary[selectedIndex];
                listDamageSummary[selectedIndex] = temp;

            }


        }
    }
}
