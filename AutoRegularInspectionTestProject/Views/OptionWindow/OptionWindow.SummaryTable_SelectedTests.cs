using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;
using AutoRegularInspection.Views;
using System.Xml.Linq;
using AutoRegularInspection;
using AutoRegularInspection.Models;

namespace AutoRegularInspectionTestProject.Views
{
    public partial class OptionWindowTests
    {
        [Fact]
        public void OptionWindow_SummaryTable_Selected_ShouldReturnCorrectVar()
        {

            //Arrange
            XDocument xDocument = XDocument.Load(App.ConfigFileName);

            //Act

            OptionWindowHelper.ExtractSummaryTableWidth(xDocument, out BridgeDeckDamageSummaryTableWidth bridgeDeckDamageSummaryTableWidth, out SuperSpaceDamageSummaryTableWidth superSpaceDamageSummaryTableWidth, out SubSpaceDamageSummaryTableWidth subSpaceDamageSummaryTableWidth);

            //Assert
            Assert.Equal(20, bridgeDeckDamageSummaryTableWidth.No);
            Assert.Equal(30, bridgeDeckDamageSummaryTableWidth.Position);
            Assert.Equal(40, bridgeDeckDamageSummaryTableWidth.Component);
            Assert.Equal(40, bridgeDeckDamageSummaryTableWidth.Damage);
            Assert.Equal(50, bridgeDeckDamageSummaryTableWidth.DamageDescription);
            Assert.Equal(20, bridgeDeckDamageSummaryTableWidth.PictureNo);
            Assert.Equal(20, bridgeDeckDamageSummaryTableWidth.Comment);
        }
    }
}
