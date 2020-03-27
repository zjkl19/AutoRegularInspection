using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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
    }
}
