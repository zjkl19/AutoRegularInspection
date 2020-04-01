using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace AutoRegularInspectionTestProject.Models
{
    public class DamageSummaryTests
    {
        [Fact]
        public void GetComponentName()
        {
            //Arrange

            var damage1 = new DamageSummary {
                ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == "伸缩缝").FirstOrDefault().Idx
            };    //Component!=其它

            var damage2 = new DamageSummary
            {
                ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx
                ,Component="其它部件"
            };    //Component==其它
            string resultExpected1 = "伸缩缝"; string resultExpected2 = "其它部件";
            //Act

            string auturalResult1 = damage1.GetComponentName(); string auturalResult2 = damage2.GetComponentName();

            //Assert
            Assert.Equal(resultExpected1, auturalResult1);
            Assert.Equal(resultExpected2, auturalResult2);
        }

        [Fact]
        public void GetDamageName()
        {
            //Arrange

            var damage1 = new DamageSummary
            {
                ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == "伸缩缝").FirstOrDefault().Idx
                ,DamageValue=GlobalData.ComponentComboBox.Where(x => x.Title == "伸缩缝").FirstOrDefault().DamageComboBox.Where(x=>x.Title=="缝内沉积物阻塞").FirstOrDefault().Idx
            };    //Damage!=其它

            var damage2 = new DamageSummary
            {
                ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == "伸缩缝").FirstOrDefault().Idx
                ,DamageValue = GlobalData.ComponentComboBox.Where(x => x.Title == "伸缩缝").FirstOrDefault().DamageComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx
                ,Damage="其它病害"
            };    //Damage==其它

            string resultExpected1 = "缝内沉积物阻塞"; string resultExpected2 = "其它病害";
            //Act

            string auturalResult1 = damage1.GetDamageName(); string auturalResult2 = damage2.GetDamageName();

            //Assert
            Assert.Equal(resultExpected1, auturalResult1);
            Assert.Equal(resultExpected2, auturalResult2);
        }
    }
}
