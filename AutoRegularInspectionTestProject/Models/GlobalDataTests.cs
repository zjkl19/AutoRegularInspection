using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AutoRegularInspectionTestProject.Models
{
    public class GlobalDataTests
    {
        [Fact]
        public void GetGlobalData()
        {
            //Arrange
           
            //Act

            //Assert
            Assert.Equal("桥面铺装", GlobalData.ComponentComboBox[0].Title);
            Assert.Equal("龟裂", GlobalData.ComponentComboBox[0].DamageComboBox[1].Title);
        }
    }
}
