using AutoRegularInspection.Models;
using NLog;
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
using System.Xml;
using System.Xml.Linq;
namespace AutoRegularInspection.Views
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        private ILogger _log;
        public OptionWindow()
        {
            InitializeComponent();
            OptionFrame.Tag = "Page1";
            OptionContentControl.DataContext = new { SubPage = new Page1() };
        }

        public OptionWindow(ILogger log)
        {
            InitializeComponent();
            OptionFrame.Tag = "Page1";
            OptionContentControl.DataContext = new { SubPage = new Page1() };
            _log = log;

        }


        private void Picture_General_Selected(object sender, RoutedEventArgs e)
        {
            if ((string)OptionFrame.Tag == nameof(OptionPicturePage))
            {
                return;
            }


            OptionFrame.Tag = nameof(OptionPicturePage);

            var config = XDocument.Load(@"Option.config");

            XElement pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            XElement pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();

            OptionContentControl.DataContext = new
            {
                SubPage = new OptionPicturePage
                {
                    DataContext = new OptionModel
                    {
                        PictureWidth = pictureWidth.Value.ToString(CultureInfo.InvariantCulture)
                        ,
                        PictureHeight = pictureHeight.Value.ToString(CultureInfo.InvariantCulture)
                    }
                }
            };
        }

        private void Report_Bookmark_Selected(object sender, RoutedEventArgs e)
        {
            if ((string)OptionFrame.Tag == nameof(OptionBookmarkPage))
            {
                return;
            }

            OptionFrame.Tag = nameof(OptionBookmarkPage);

            var config = XDocument.Load(@"Option.config");

            var BridgeDeckBookmarkStartNo = config.Elements("configuration").Elements("Bookmark").Elements("BridgeDeckBookmarkStartNo").FirstOrDefault();
            var SuperSpaceBookmarkStartNo = config.Elements("configuration").Elements("Bookmark").Elements("SuperSpaceBookmarkStartNo").FirstOrDefault();
            var SubSpaceBookmarkStartNo = config.Elements("configuration").Elements("Bookmark").Elements("SubSpaceBookmarkStartNo").FirstOrDefault();

            OptionContentControl.DataContext = new
            {
                SubPage = new OptionBookmarkPage
                {
                    DataContext = new OptionModel
                    {
                        BridgeDeckBookmarkStartNo = BridgeDeckBookmarkStartNo.Value.ToString(CultureInfo.InvariantCulture)
                        ,
                        SuperSpaceBookmarkStartNo = SuperSpaceBookmarkStartNo.Value.ToString(CultureInfo.InvariantCulture)
                        ,
                        SubSpaceBookmarkStartNo = SubSpaceBookmarkStartNo.Value.ToString(CultureInfo.InvariantCulture)
                    }
                }
            };
        }

        private void TreeViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            OptionFrame.Tag = "Page1";
            OptionContentControl.DataContext = new { SubPage = new Page1() };

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
