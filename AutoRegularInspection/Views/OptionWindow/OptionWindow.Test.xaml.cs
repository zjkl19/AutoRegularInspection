using AutoRegularInspection.Models;
using System.Windows;
using System.Windows.Controls;

namespace AutoRegularInspection.Views
{
    public partial class OptionWindow : Window
    {
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = OptionContentControl.Content as Frame;


            //var m = (Page1)frame.Content;

            //MessageBox.Show((string)frame.Tag);

            var m = (OptionPicturePage)frame.Content;
            var m1 = (m.DataContext) as OptionModel;
            MessageBox.Show(m1.PictureHeight);

            //if((string)fr.Tag=="Page1")
            //{
            //    var m = (Page1)fr.Content;
            //    MessageBox.Show($"{m.TxtInfo.Text}");
            //}
        }
    }
}
