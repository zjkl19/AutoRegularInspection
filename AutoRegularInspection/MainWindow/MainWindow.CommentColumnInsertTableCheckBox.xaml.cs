using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow
    {
        private void CommentColumnInsertTableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (CommentColumnInsertTableCheckBox.IsChecked ?? false)
                {
                    appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value = "true";
                }
                else
                {
                    appConfig.AppSettings.Settings["CommentColumnInsertTable"].Value = "false";
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
