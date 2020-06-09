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
            ObservableCollection<DamageSummary> listDamageSummary = dg.ItemsSource as ObservableCollection<DamageSummary>;
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
            if (selectedIndex >= 0)    //判断是否有选中的行
            {
                listDamageSummary = dg.ItemsSource as ObservableCollection<DamageSummary>;
                listDamageSummary.Add(listDamageSummary[selectedIndex]);
            }

            GetDataGridRow(dg, listDamageSummary.Count - 1);

        }
        //名为GetDataGridRow，实际上作用是选中最后一行
        //Bug:复制比较靠前的行会报错，因为ContainerFromIndex(总行数-1)可能会由于视图太窄的原因找不到最后一行。
        private DataGridRow GetDataGridRow(DataGrid datagrid, int rowIndex)
        {
            DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);

            if (row == null)
            {
                datagrid.UpdateLayout();
                //datagrid.ScrollIntoView(datagrid.Items[rowIndex]);
                row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
                
                row.IsSelected = true;

                //滚动到最后一行
                var border = System.Windows.Media.VisualTreeHelper.GetChild(datagrid, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }

            }
            return row;
        }
    }
}
