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
                var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (DeletePositionInBridgeDeckCheckBox.IsChecked ?? false)
                {
                    appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value = "true";
                }
                else
                {
                    appConfig.AppSettings.Settings["DeletePositionInBridgeDeck"].Value = "false";
                }

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
