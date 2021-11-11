using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;
using AutoRegularInspection.Views;
namespace AutoRegularInspectionTestProject.Views
{
    public partial class OptionWindowTests
    {
        [Fact]
        public void OptionWindow_SummaryTable_Selected_ShouldReturnCorrectVar()
        {

            //Arrange

            
            //Act

            OptionWindowHelper.GetSummaryTableWidthXmlNodeList(out XmlNodeList bridgeDeckGrouplist, out XmlNodeList superSpaceGrouplist, out XmlNodeList subSpaceGrouplist);

            //Assert
            Assert.Equal("No", bridgeDeckGrouplist[0].Attributes["key"].Value);
            Assert.Equal("20", bridgeDeckGrouplist[0].Attributes["value"].Value);
            Assert.Equal("Position", bridgeDeckGrouplist[1].Attributes["key"].Value);
            Assert.Equal("30", bridgeDeckGrouplist[1].Attributes["value"].Value);
            Assert.Equal("40", bridgeDeckGrouplist[2].Attributes["value"].Value);

        }
    }


}
