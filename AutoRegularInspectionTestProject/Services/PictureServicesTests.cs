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
        public void ValidatePicturesTest()
        {
            //Arrange
            var bridgeDeckListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875"
                }
            };
            var superSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875i"
                }
            };
            var subSpaceListDamageSummary = new List<DamageSummary>
            {
                new DamageSummary {
                    PictureNo="855,858,875i"
                }
            };

            //Act
            int totalInvalidPictureCounts = PictureServices.ValidatePictures(bridgeDeckListDamageSummary, superSpaceListDamageSummary, subSpaceListDamageSummary, out List<string> bridgeDeckValidationResult, out List<string> superSpaceValidationResult, out List<string> subSpaceValidationResult);

            //Assert
            Assert.Equal(2, totalInvalidPictureCounts);
        }
    }
}
