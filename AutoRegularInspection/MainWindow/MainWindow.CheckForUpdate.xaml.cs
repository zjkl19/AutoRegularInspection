
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            StatusBarText.Text = "正在检查更新……";
            Repository.CheckForUpdate.CheckByRestClient();
            StatusBarText.Text = "就绪";
        }
    }
}
