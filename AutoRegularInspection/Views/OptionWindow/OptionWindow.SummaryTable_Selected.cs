using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace AutoRegularInspection.Views
{
    public partial class OptionWindow : Window
    {
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

            //OptionFrame.Tag = nameof(OptionSummaryTablePage);

            //var config = XDocument.Load(App.ConfigFileName);


            //var BridgeDeckBookmarkStartNo = config.Elements("configuration").Elements("BridgeDeckSummaryTable").GetAtt;

            OptionWindowHelper.GetSummaryTableWidthXmlNodeList(out XmlNodeList bridgeDeckGrouplist, out XmlNodeList superSpaceGrouplist, out XmlNodeList subSpaceGrouplist);

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


        }
    }

    public static partial class OptionWindowHelper
    {
        public static void GetSummaryTableWidthXmlNodeList(out XmlNodeList bridgeDeckGrouplist, out XmlNodeList superSpaceGrouplist, out XmlNodeList subSpaceGrouplist)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename: App.ConfigFileName);
            bridgeDeckGrouplist = xmlDoc.SelectSingleNode("configuration").SelectSingleNode("BridgeDeckSummaryTable").ChildNodes;
            superSpaceGrouplist = xmlDoc.SelectSingleNode("configuration").SelectSingleNode("SuperSpaceSummaryTable").ChildNodes;
            subSpaceGrouplist = xmlDoc.SelectSingleNode("configuration").SelectSingleNode("SubSpaceSummaryTable").ChildNodes;
        }
    }
}
