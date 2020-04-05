using AutoRegularInspection.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            int testRow = 5;
            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            //_bridgeDeckListDamageSummary[0].DamageDescription = "lbt";

            //_bridgeDeckListDamageSummary[2].DamageComboBox = GlobalData.ComponentComboBox[5].DamageComboBox;
            //_bridgeDeckListDamageSummary[2].DamageValue = 2;
            //MessageBox.Show(_bridgeDeckListDamageSummary[2].Component);

            try
            {

                MessageBox.Show(_bridgeDeckListDamageSummary[testRow].ComponentValue.ToString());
                MessageBox.Show(_bridgeDeckListDamageSummary[testRow].DamageValue.ToString());
                MessageBox.Show(_bridgeDeckListDamageSummary[testRow].Component.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
