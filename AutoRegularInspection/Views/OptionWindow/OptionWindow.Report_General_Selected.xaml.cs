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
        private void Report_General_Selected(object sender, RoutedEventArgs e)
        {
            if ((string)OptionFrame.Tag == nameof(OptionGeneralPage))
            {
                return;
            }

            OptionFrame.Tag = nameof(OptionGeneralPage);

            //XDocument xDocument = XDocument.Load(App.ConfigFileName);

            OptionContentControl.DataContext = new
            {
                SubPage = new OptionGeneralPage()
                ,
            };
        }
    }
}
