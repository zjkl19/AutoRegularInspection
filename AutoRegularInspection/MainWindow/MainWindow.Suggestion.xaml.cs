using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using AutoRegularInspection.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        //免责声明
        private void SuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new SuggestionWindow();
            w.Top = 0.4 * (App.ScreenHeight - w.Height);
            w.Left = 0.5 * (App.ScreenWidth - w.Width);
            w.Show();

            var s = new SuggestionServices();

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;

            var lst = _bridgeDeckListDamageSummary.Union(_superSpaceListDamageSummary).Union(_subSpaceListDamageSummary).ToList();

            w.SuggestionTextBox.Text = s.MakeSuggestions(lst);

        }
    }
}
