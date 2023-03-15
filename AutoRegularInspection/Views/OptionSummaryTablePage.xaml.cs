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
        public OptionSummaryTablePage(BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth, SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth, SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth)
        {
            //TODO:增加校验
            //if (bridgeDeckGrouplist is null)
            //{
            //    throw new ArgumentNullException(nameof(bridgeDeckGrouplist));
            //}

            InitializeComponent();

            BridgeDeckStackPanel.DataContext = new BridgeDeckDamageSummaryTableWidth
            {

                No = bridgeDeckDamageSummaryTableWidth.No
                ,
                Position = bridgeDeckDamageSummaryTableWidth.PictureNo
                ,
                Component = bridgeDeckDamageSummaryTableWidth.Component
                ,
                Damage = bridgeDeckDamageSummaryTableWidth.Damage
                ,
                DamagePosition = bridgeDeckDamageSummaryTableWidth.DamagePosition
                ,
                DamageDescription = bridgeDeckDamageSummaryTableWidth.DamageDescription
                ,
                PictureNo = bridgeDeckDamageSummaryTableWidth.PictureNo
                ,
                Comment = bridgeDeckDamageSummaryTableWidth.Comment
            };

            SuperSpaceStackPanel.DataContext = new SuperSpaceDamageSummaryTableWidth
            {

                No = superSpaceDamageSummaryTableWidth.No
                ,
                Position = superSpaceDamageSummaryTableWidth.PictureNo
                ,
                Component = superSpaceDamageSummaryTableWidth.Component
                ,
                Damage = superSpaceDamageSummaryTableWidth.Damage
                ,
                DamagePosition = superSpaceDamageSummaryTableWidth.DamagePosition
                ,
                DamageDescription = superSpaceDamageSummaryTableWidth.DamageDescription
                ,
                PictureNo = superSpaceDamageSummaryTableWidth.PictureNo
                ,
                Comment = superSpaceDamageSummaryTableWidth.Comment
            };
            SubSpaceStackPanel.DataContext = new SubSpaceDamageSummaryTableWidth
            {


                No = subSpaceDamageSummaryTableWidth.No
                ,
                Position = subSpaceDamageSummaryTableWidth.PictureNo
                ,
                Component = subSpaceDamageSummaryTableWidth.Component
                ,
                Damage = subSpaceDamageSummaryTableWidth.Damage
                ,
                DamagePosition = subSpaceDamageSummaryTableWidth.DamagePosition
                ,
                DamageDescription = subSpaceDamageSummaryTableWidth.DamageDescription
                ,
                PictureNo = subSpaceDamageSummaryTableWidth.PictureNo
                ,
                Comment = subSpaceDamageSummaryTableWidth.Comment
            };
        }
    }
}
