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
    /// OptionGeneralPage.xaml 的交互逻辑
    /// </summary>
    public partial class OptionGeneralPage :Page
    {
        public OptionGeneralPage()
        {
            InitializeComponent();

            XDocument xDocument = XDocument.Load($"{App.ConfigurationFolder}\\{App.ConfigFileName}");

            OptionGeneralPageStackPanel.DataContext = new OptionReportGeneralSettings
            {
                IntactStructNoInsertSummaryTable = Convert.ToBoolean(xDocument.Elements("configuration").Elements("General").Elements("IntactStructNoInsertSummaryTable").FirstOrDefault().Value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)
                ,
            };
            //IntactStructNoInsertSummaryTableCheckBox.DataContext= new OptionReportGeneralSettings { IntactStructNoInsertSummaryTable = true };
             
        }
    }
}
