﻿using AutoRegularInspection.Models;
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

            var config = XDocument.Load(App.ConfigFileName);

            if ((string)OptionFrame.Tag == nameof(OptionPicturePage))
            {
                OptionPicturePage frameContent = (OptionPicturePage)frame.Content;
                OptionModel model = frameContent.DataContext as OptionModel;

                XElement pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
                pictureWidth.Value = model.PictureWidth;
                XElement pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
                pictureHeight.Value = model.PictureHeight;

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
                BridgeDeckDamageSummaryTableWidth bridgeDeckmodel = frameContent.BridgeDeckStackPanel.DataContext as BridgeDeckDamageSummaryTableWidth;
                
            }

            config.Save(App.ConfigFileName);
            _ = MessageBox.Show("保存设置成功！");
            //try
            //{

            //    var config = XDocument.Load(@"Option.config");

            //    var pictureWidth = config.Elements("configuration").Elements("Picture").Elements("Width").FirstOrDefault();
            //    pictureWidth.Value = PictureWidth.Text;
            //    var pictureHeight = config.Elements("configuration").Elements("Picture").Elements("Height").FirstOrDefault();
            //    pictureHeight.Value = PictureHeight.Text;
            //    config.Save(@"Option.config");

            //    MessageBox.Show("保存设置成功！");
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

    }
}