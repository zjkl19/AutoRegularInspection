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
        /// 下移选中行一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveRowDown_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = BridgeDeckGrid;

            ObservableCollection<BridgeDamage> componentBox = GlobalData.ComponentComboBox;

            if (BridgeDeckTabItem.IsSelected)
            {
                dg = BridgeDeckGrid;
                componentBox = GlobalData.ComponentComboBox;
            }
            else if (SuperSpaceTabItem.IsSelected)
            {
                dg = SuperSpaceGrid;
                componentBox = GlobalData.SuperSpaceComponentComboBox;
            }
            else if (SubSpaceTabItem.IsSelected)
            {
                dg = SubSpaceGrid;
                componentBox = GlobalData.SuperSpaceComponentComboBox;
            }
            DamageSummary temp1,temp2;
            int c1,d1,c2,d2;
            int selectedIndex = dg.SelectedIndex;
            ObservableCollection<DamageSummary> listDamageSummary = dg.ItemsSource as ObservableCollection<DamageSummary>;
            if (selectedIndex < listDamageSummary.Count-1)
            {
                
                temp1 = listDamageSummary[selectedIndex + 1];
                c1 = temp1.ComponentValue;
                d1 = temp1.DamageValue;
                temp2= listDamageSummary[selectedIndex];
                c2 = temp2.ComponentValue;
                d2 = temp2.DamageValue;
                listDamageSummary[selectedIndex + 1] = temp2;
                listDamageSummary[selectedIndex + 1].ComponentValue = c2;
                listDamageSummary[selectedIndex + 1].DamageValue = d2;
                listDamageSummary[selectedIndex] = temp1;
                listDamageSummary[selectedIndex].ComponentValue = c1;
                listDamageSummary[selectedIndex].DamageValue = d1;

                //TODO:数据绑定DamageComboBox
                //由于没有converter，这里要手动进行转换
                //listDamageSummary[selectedIndex - 1].ComponentValue = listDamageSummary[selectedIndex].ComponentValue;
                //listDamageSummary[selectedIndex - 1].DamageValue = listDamageSummary[selectedIndex].DamageValue;

                //listDamageSummary[selectedIndex - 1].DamageComboBox = componentBox[listDamageSummary[selectedIndex].ComponentValue].DamageComboBox;
                //istDamageSummary[selectedIndex - 1].DamageValue = 2;

                //listDamageSummary[selectedIndex] = temp;
                //由于没有converter，这里要手动进行转换
                //listDamageSummary[selectedIndex].DamageComboBox = componentBox[temp.ComponentValue].DamageComboBox;

                //listDamageSummary[selectedIndex].ComponentValue = temp.ComponentValue;
                //listDamageSummary[selectedIndex].DamageValue = temp.DamageValue;


                //选中移动后的行
                var row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(selectedIndex + 1);
                row.IsSelected = true;

            }
      

        }
    }
}
