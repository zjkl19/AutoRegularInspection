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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace AutoRegularInspection.Views
{
    /// <summary>
    /// OptionSummaryTable.xaml 的交互逻辑
    /// </summary>
    public partial class OptionSummaryTablePage : Page
    {
        public OptionSummaryTablePage()
        {
            InitializeComponent();
        }
        public OptionSummaryTablePage(XmlNodeList grouplist)
        {
            if (grouplist is null)
            {
                throw new ArgumentNullException(nameof(grouplist));
            }

            InitializeComponent();

            BridgeDeckStackPanel.DataContext = new BridgeDeckDamageSummaryTableWidth
            {

                No = Convert.ToInt32(grouplist[0].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Position = Convert.ToInt32(grouplist[1].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Component = Convert.ToInt32(grouplist[2].Attributes["value"].Value, CultureInfo.InvariantCulture)
            };
        }
    }
}
