using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Xunit;

namespace AutoRegularInspectionTestProject.Services
{
    public class PictureServicesTests
    {
        //TODO：修改测试文件名
        [Fact]
        public void ValidatePicturesTest_ShouldReturnCorrectInvalidPictureCounts_And_ValidationResult()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855;858;875"
                }
            };
            var superSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855;858;875x"
                }
            };
            var subSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855;858y;875z"
                }
            };

            DamageSummaryServices.InitListDamageSummary(bridgeDeckListDamageSummary);
            DamageSummaryServices.InitListDamageSummary(superSpaceListDamageSummary, 2_000_000, BridgePart.SuperSpace);
            DamageSummaryServices.InitListDamageSummary(subSpaceListDamageSummary, 3_000_000, BridgePart.SubSpace);

            //Act
            int totalInvalidPictureCounts = PictureServices.ValidatePictures(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary, out List<string> bridgeDeckValidationResult, out List<string> superSpaceValidationResult, out List<string> subSpaceValidationResult);

            //Assert
            Assert.Equal(3, totalInvalidPictureCounts);
            Assert.Equal("875x", superSpaceValidationResult[0]);
        }
    }
}
