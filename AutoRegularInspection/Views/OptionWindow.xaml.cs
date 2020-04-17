using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AutoRegularInspection.Views
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow()
        {
            InitializeComponent();
            OptionFrame.Tag = "Page1";
            OptionContentControl.DataContext = new { SubPage = new Page1() };

            var config = XDocument.Load(@"Option.config");
            try
            {
                var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
                PictureWidth.Text = pictureWidth.Value.ToString(CultureInfo.InvariantCulture);
                var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
                PictureHeight.Text = pictureHeight.Value.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                //TODO:数据格式不正确时的异常处理
                throw ex;
            }

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var config = XDocument.Load(@"Option.config");

                var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
                pictureWidth.Value = PictureWidth.Text;
                var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
                pictureHeight.Value = PictureHeight.Text;
                config.Save(@"Option.config");

                MessageBox.Show("保存设置成功！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = OptionContentControl.Content as Frame;


            //var m = (Page1)frame.Content;

            MessageBox.Show((string)frame.Tag);

            //if((string)fr.Tag=="Page1")
            //{
            //    var m = (Page1)fr.Content;
            //    MessageBox.Show($"{m.TxtInfo.Text}");
            //}
        }

        private void Picture_General_Selected(object sender, RoutedEventArgs e)
        {
            OptionFrame.Tag = nameof(Page2);
            OptionContentControl.DataContext = new { SubPage = new Page2() };
        }

        private void TreeViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            OptionFrame.Tag = "Page1";
            OptionContentControl.DataContext = new { SubPage= new Page1 () };

            //TestContentControl.Content = new Frame
            //{
            //    Content = new Page1()
            //    ,
            //    Tag = "Page1"
            //};
        }

        private void TreeViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            OptionFrame.Tag = nameof(Page2);
            OptionContentControl.DataContext = new { SubPage = new Page2() };

            //TestContentControl.Content = new Frame
            //{
            //    Content = new Page2()
            //    ,Tag = "Page2"
            //};
        }
    }
}
