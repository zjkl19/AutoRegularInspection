using AutoRegularInspection.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AutoRegularInspection.Views
{
    public partial class OptionWindow : Window
    {

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = OptionContentControl.Content as Frame;

            var config = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");

            if ((string)OptionFrame.Tag == nameof(OptionPicturePage))
            {
                OptionPicturePage frameContent = (OptionPicturePage)frame.Content;
                OptionModel model = frameContent.DataContext as OptionModel;

                XElement pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
                pictureWidth.Value = model.PictureWidth;
                XElement pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
                pictureHeight.Value = model.PictureHeight;
                XElement pictureMaxCompressSize = config.Elements("configuration").Elements("Picture").Elements("MaxCompressSize").FirstOrDefault();
                pictureMaxCompressSize.Value = model.PictureMaxCompressSize;
                XElement pictureCompressQuality = config.Elements("configuration").Elements("Picture").Elements("CompressQuality").FirstOrDefault();
                pictureCompressQuality.Value = model.PictureCompressQuality;

            }
            else if ((string)OptionFrame.Tag == nameof(OptionBookmarkPage))
            {
                OptionBookmarkPage frameContent = (OptionBookmarkPage)frame.Content;
                OptionModel model = frameContent.DataContext as OptionModel;
                XElement BridgeDeckBookmarkStartNo = config.Elements("configuration").Elements("Bookmark").Elements("BridgeDeckBookmarkStartNo").FirstOrDefault();
                XElement SuperSpaceBookmarkStartNo = config.Elements("configuration").Elements("Bookmark").Elements("SuperSpaceBookmarkStartNo").FirstOrDefault();
                XElement SubSpaceBookmarkStartNo = config.Elements("configuration").Elements("Bookmark").Elements("SubSpaceBookmarkStartNo").FirstOrDefault();

                BridgeDeckBookmarkStartNo.Value = model.BridgeDeckBookmarkStartNo;
                SuperSpaceBookmarkStartNo.Value = model.SuperSpaceBookmarkStartNo;
                SubSpaceBookmarkStartNo.Value = model.SubSpaceBookmarkStartNo;
            }
            else if ((string)OptionFrame.Tag == nameof(OptionSummaryTablePage))
            {
                //TODO:修改
                OptionSummaryTablePage frameContent = (OptionSummaryTablePage)frame.Content;
                BridgeDeckDamageSummaryTableWidth bridgeDeckModel = frameContent.BridgeDeckStackPanel.DataContext as BridgeDeckDamageSummaryTableWidth;
                SuperSpaceDamageSummaryTableWidth superSpaceModel = frameContent.SuperSpaceStackPanel.DataContext as SuperSpaceDamageSummaryTableWidth;
                SubSpaceDamageSummaryTableWidth subSpaceModel = frameContent.SubSpaceStackPanel.DataContext as SubSpaceDamageSummaryTableWidth;
                SetSummaryTableWidth(config, bridgeDeckModel, "BridgeDeckSummaryTable");
                SetSummaryTableWidth(config, superSpaceModel, "SuperSpaceSummaryTable");
                SetSummaryTableWidth(config, subSpaceModel, "SubSpaceSummaryTable");
            }

            else if ((string)OptionFrame.Tag == nameof(OptionGeneralPage))
            {
                //TODO:修改
                OptionGeneralPage frameContent = (OptionGeneralPage)frame.Content;
                var model = frameContent.OptionGeneralPageStackPanel.DataContext as OptionReportGeneralSettings;
                XElement IntactStructNoInsertSummaryTable = config.Elements("configuration").Elements("General").Elements("IntactStructNoInsertSummaryTable").FirstOrDefault();
                IntactStructNoInsertSummaryTable.Value = model.IntactStructNoInsertSummaryTable.ToString(CultureInfo.InvariantCulture);
            }
            config.Save($"{App.ConfigurationFolder}\\{App.ConfigFileName}");
            _ = MessageBox.Show("保存设置成功！");

            void SetSummaryTableWidth(XDocument xDocument, BridgeDamageSummaryTableWidth model,string elementName)
            {
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("No").Value = model.No.ToString(CultureInfo.InvariantCulture);
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("Position").Value = model.Position.ToString(CultureInfo.InvariantCulture);
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("Component").Value = model.Component.ToString(CultureInfo.InvariantCulture);
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("Damage").Value = model.Damage.ToString(CultureInfo.InvariantCulture);
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("DamageDescription").Value = model.DamageDescription.ToString(CultureInfo.InvariantCulture);
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("PictureNo").Value = model.PictureNo.ToString(CultureInfo.InvariantCulture);
                config.Elements("configuration").Elements(elementName).FirstOrDefault().Attribute("Comment").Value = model.Comment.ToString(CultureInfo.InvariantCulture);
            }
            
        }

    }
}
