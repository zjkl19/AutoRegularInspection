using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace AutoRegularInspectionTestProject.Services
{
    public class DamageSummaryServices
    {
        [Fact]
        public void SetPictureCounts_ShouldSetCorretPictureCounts()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875"
                }
            };

            //Act

            AutoRegularInspection.Services.DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);

            //Assert
            Assert.Equal(3, bridgeDeckListDamageSummary[0].PictureCounts);
        }

        [Fact]
        public void SetFirstAndLastPictureBookmark_ShouldSetCorretBookmarkName()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875"
                }
            };

           
            //Act

            AutoRegularInspection.Services.DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);

            //Assert
            Assert.Equal(1000000, bridgeDeckListDamageSummary[0].FirstPictureBookmarkIndex);
            Assert.Equal($"_Ref1000000", bridgeDeckListDamageSummary[0].FirstPictureBookmark);
            Assert.Equal($"_Ref1000002", bridgeDeckListDamageSummary[0].LastPictureBookmark);
        }

        [Fact]
        public void SetComboBox_ShouldSetCorretComponentValue()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="伸缩缝"
                    ,Damage="缝内沉积物阻塞"
                    ,PictureNo="855,858,875"
                }
            };

            //Act

            AutoRegularInspection.Services.DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);
            //Assert
            Assert.Equal(2, bridgeDeckListDamageSummary[0].ComponentValue);
        }
        /// <summary>
        /// WhileDamageFound指的是在非“其它”的“枚举”中找到
        /// </summary>
        [Fact]
        public void SetComboBox_ShouldSetCorrectDamageAndDamageValue_WhileDamageFound()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="伸缩缝"
                    ,Damage="缝内沉积物阻塞"
                    ,PictureNo="855,858,875"
                }
            };

            //Act

            AutoRegularInspection.Services.DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);
            //Assert
            Assert.Equal("螺帽松动", bridgeDeckListDamageSummary[0].DamageComboBox[0].Title);
            Assert.Equal("缝内沉积物阻塞", bridgeDeckListDamageSummary[0].DamageComboBox[1].Title);
            Assert.Equal(1, bridgeDeckListDamageSummary[0].DamageValue);
        }

  
        [Fact]
        public void SetComboBox_ShouldSetCorrectDamageAndDamageValue_WhileDamageNotFound()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {

                    Component="伸缩缝"
                    ,Damage="其它病害"
                    ,PictureNo="855,858,875"
                }
            };

            //Act

            AutoRegularInspection.Services.DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);
            //Assert
            Assert.Equal("螺帽松动", bridgeDeckListDamageSummary[0].DamageComboBox[0].Title);
            Assert.Equal("缝内沉积物阻塞", bridgeDeckListDamageSummary[0].DamageComboBox[1].Title);
            Assert.Equal(10, bridgeDeckListDamageSummary[0].DamageValue);
        }
    }
}
