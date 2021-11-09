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
using System.Xml;
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
        }


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

        /// <summary>
        /// 选项=>报告=>汇总表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SummaryTable_Selected(object sender, RoutedEventArgs e)
        {
            if ((string)OptionFrame.Tag == nameof(OptionSummaryTablePage))
            {
                return;
            }

            OptionFrame.Tag = nameof(OptionSummaryTablePage);

            var config = XDocument.Load(@"Option.config");


            //var BridgeDeckBookmarkStartNo = config.Elements("configuration").Elements("BridgeDeckSummaryTable").GetAtt;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"Option.config");
            XmlNodeList bridgeDeckGrouplist = xmlDoc.SelectSingleNode("configuration").SelectSingleNode("BridgeDeckSummaryTable").ChildNodes;
            XmlNodeList superSpaceGrouplist = xmlDoc.SelectSingleNode("configuration").SelectSingleNode("SuperSpaceSummaryTable").ChildNodes;
            XmlNodeList subSpaceGrouplist = xmlDoc.SelectSingleNode("configuration").SelectSingleNode("SubSpaceSummaryTable").ChildNodes;

            //TODO:研究以下几行
            //foreach (XmlNode xng in bridgeDeckGrouplist)
            //{
            //    XmlElement xeg = (XmlElement)xng;
            //    string width = xeg.GetAttribute("value");
            //}

            OptionContentControl.DataContext = new
            {
                SubPage = new OptionSummaryTablePage(bridgeDeckGrouplist, superSpaceGrouplist, subSpaceGrouplist)
            };

            //OptionContentControl.DataContext = new
            //{
            //    SubPage = new OptionSummaryTablePage
            //    {

            //        DataContext = new BridgeDeckDamageSummaryTableWidth
            //        {
            //            No = Convert.ToInt32(grouplist[0].Attributes["value"].Value, CultureInfo.InvariantCulture)
            //,
            //            Position = Convert.ToInt32(grouplist[1].Attributes["value"].Value, CultureInfo.InvariantCulture)
            //        }

            //        //DataContext = new OptionModel
            //        //{
            //        //    BridgeDeckNoWidth = grouplist[0].Attributes["value"].Value.ToString(CultureInfo.InvariantCulture)
            //        //    ,
            //        //    BridgeDeckPositionWidth = grouplist[1].Attributes["value"].Value.ToString(CultureInfo.InvariantCulture)
            //        //}
            //    }
            //};
        }


        private void Picture_General_Selected(object sender, RoutedEventArgs e)
        {
            if ((string)OptionFrame.Tag == nameof(OptionPicturePage))
            {
                return;
            }

            OptionFrame.Tag = nameof(OptionPicturePage);

            var config = XDocument.Load(@"Option.config");

            var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();

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
