using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow
    {
        private void DeletePositionInBridgeDeckCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value = DeletePositionInBridgeDeckCheckBox.IsChecked ?? false ? "true" : "false";

                appConfig.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

        }

        private void DeletePositionInSuperSpaceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                appConfig.AppSettings.Settings["DeletePositionInSuperSpace"].Value = DeletePositionInSuperSpaceCheckBox.IsChecked ?? false ? "true" : "false";

                appConfig.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

        }
    }
}
