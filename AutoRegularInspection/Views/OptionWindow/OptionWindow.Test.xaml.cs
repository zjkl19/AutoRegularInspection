using AutoRegularInspection.Models;
using AutoRegularInspection.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AutoRegularInspection.Views
{
    public partial class OptionWindow : Window
    {
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (OptionViewModel)DataContext;
            var a = viewModel.Options[0].UserControl.DataContext as OptionConfiguration;
            var b = 1;
        }
    }
}
