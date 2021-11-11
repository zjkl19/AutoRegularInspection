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
        public OptionSummaryTablePage(XmlNodeList bridgeDeckGrouplist, XmlNodeList superSpaceGrouplist,XmlNodeList subSpaceGrouplist)
        {
            //TODO:增加校验
            if (bridgeDeckGrouplist is null)
            {
                throw new ArgumentNullException(nameof(bridgeDeckGrouplist));
            }

            InitializeComponent();

            BridgeDeckStackPanel.DataContext = new BridgeDeckDamageSummaryTableWidth
            {

                No = Convert.ToInt32(bridgeDeckGrouplist[0].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Position = Convert.ToInt32(bridgeDeckGrouplist[1].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Component = Convert.ToInt32(bridgeDeckGrouplist[2].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Damage = Convert.ToInt32(bridgeDeckGrouplist[3].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,DamageDescription = Convert.ToInt32(bridgeDeckGrouplist[4].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,PictureNo = Convert.ToInt32(bridgeDeckGrouplist[5].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Comment = Convert.ToInt32(bridgeDeckGrouplist[6].Attributes["value"].Value, CultureInfo.InvariantCulture)
            };

            SuperSpaceStackPanel.DataContext = new BridgeDeckDamageSummaryTableWidth
            {

                No = Convert.ToInt32(superSpaceGrouplist[0].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Position = Convert.ToInt32(superSpaceGrouplist[1].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Component = Convert.ToInt32(superSpaceGrouplist[2].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Damage = Convert.ToInt32(superSpaceGrouplist[3].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,DamageDescription = Convert.ToInt32(superSpaceGrouplist[4].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,PictureNo = Convert.ToInt32(superSpaceGrouplist[5].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Comment = Convert.ToInt32(superSpaceGrouplist[6].Attributes["value"].Value, CultureInfo.InvariantCulture)
            };
            SubSpaceStackPanel.DataContext = new BridgeDeckDamageSummaryTableWidth
            {

                No = Convert.ToInt32(superSpaceGrouplist[0].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,Position = Convert.ToInt32(superSpaceGrouplist[1].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,
                Component = Convert.ToInt32(superSpaceGrouplist[2].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,
                Damage = Convert.ToInt32(superSpaceGrouplist[3].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,
                DamageDescription = Convert.ToInt32(superSpaceGrouplist[4].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,
                PictureNo = Convert.ToInt32(superSpaceGrouplist[5].Attributes["value"].Value, CultureInfo.InvariantCulture)
                ,
                Comment = Convert.ToInt32(superSpaceGrouplist[6].Attributes["value"].Value, CultureInfo.InvariantCulture)
            };
        }
    }
}
