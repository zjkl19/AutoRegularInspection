using AutoRegularInspection.Models;
using System;
using System.Globalization;
using System.Linq;
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

            OptionFrame.Tag = nameof(OptionSummaryTablePage);

            //var config = XDocument.Load(App.ConfigFileName);

            //var BridgeDeckBookmarkStartNo = config.Elements("configuration").Elements("BridgeDeckSummaryTable").GetAtt;

            XDocument xDocument = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");
            OptionWindowHelper.ExtractSummaryTableWidth(xDocument, out BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth, out SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth, out SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth);

            //TODO:研究以下几行
            //foreach (XmlNode xng in bridgeDeckGrouplist)
            //{
            //    XmlElement xeg = (XmlElement)xng;
            //    string width = xeg.GetAttribute("value");
            //}

            OptionContentControl.DataContext = new
            {
                SubPage = new OptionSummaryTablePage(bridgeDeckDamageSummaryTableWidth, superSpaceDamageSummaryTableWidth, subSpaceDamageSummaryTableWidth)
            };

        }
    }

    public static partial class OptionWindowHelper
    {
        public static void ExtractSummaryTableWidth(XDocument xDocument,out BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth,out SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth,out SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth)
        {
            //No="20" Position="30" Component="40" Damage="40" DamageDescription="50" PictureNo="20" Comment="20"
            bridgeDeckDamageSummaryTableWidth = new BridgeDeckDamageSummaryTableWidth
            {
                No = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("No").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
                Position = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("Position").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
                Component = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("Component").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
                Damage = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("Damage").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
                DamageDescription = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("DamageDescription").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
                PictureNo = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("PictureNo").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
                Comment = Convert.ToDouble(xDocument.Elements("configuration").Elements("BridgeDeckSummaryTable").FirstOrDefault().Attribute("Comment").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
            };
            superSpaceDamageSummaryTableWidth = new SuperSpaceDamageSummaryTableWidth
            {
                No = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("No").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,Position = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("Position").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,Component = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("Component").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,Damage = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("Damage").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,DamageDescription = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("DamageDescription").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,PictureNo = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("PictureNo").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,Comment = Convert.ToDouble(xDocument.Elements("configuration").Elements("SuperSpaceSummaryTable").FirstOrDefault().Attribute("Comment").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
            };
            subSpaceDamageSummaryTableWidth = new SubSpaceDamageSummaryTableWidth
            {
                No = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("No").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
    ,
                Position = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("Position").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
    ,
                Component = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("Component").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
    ,
                Damage = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("Damage").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
    ,
                DamageDescription = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("DamageDescription").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
    ,
                PictureNo = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("PictureNo").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
    ,
                Comment = Convert.ToDouble(xDocument.Elements("configuration").Elements("SubSpaceSummaryTable").FirstOrDefault().Attribute("Comment").Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
            };
        }
    }
}
