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
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            int testRow = 1;
            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            //_bridgeDeckListDamageSummary[0].DamageDescription = "lbt";

            //_bridgeDeckListDamageSummary[2].DamageComboBox = GlobalData.ComponentComboBox[5].DamageComboBox;
            //_bridgeDeckListDamageSummary[2].DamageValue = 2;
            //MessageBox.Show(_bridgeDeckListDamageSummary[2].Component);

            try
            {
                //MessageBox.Show(_bridgeDeckListDamageSummary[testRow].ComponentValue.ToString(CultureInfo.InvariantCulture));
                MessageBox.Show(_bridgeDeckListDamageSummary[testRow].DamageValue.ToString(CultureInfo.InvariantCulture));
                //MessageBox.Show(_bridgeDeckListDamageSummary[testRow].Component.ToString(CultureInfo.InvariantCulture));
                //MessageBox.Show(_bridgeDeckListDamageSummary[testRow].SeverityQuality.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString(CultureInfo.InvariantCulture));
            }


            _bridgeDeckListDamageSummary[1].DamageValue = 9;
            //var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            //_bridgeDeckListDamageSummary[0].DamageDescription = "lbt";

            //_bridgeDeckListDamageSummary[2].DamageComboBox = GlobalData.ComponentComboBox[5].DamageComboBox;
            //_bridgeDeckListDamageSummary[2].DamageValue = 2;


        }


    }
}
