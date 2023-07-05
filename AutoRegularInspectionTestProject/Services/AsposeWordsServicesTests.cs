using AutoRegularInspection;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AutoRegularInspectionTestProject.Services
{
    public class AsposeWordsServicesTests
    {
        [Fact]
        public void GetTotalPictureCounts_EmptyList_ReturnsZero()
        {
            // Arrange
            var listDamageSummary = new List<DamageSummary>();

            // Act
            int result = AsposeWordsServices.GetTotalPictureCounts(listDamageSummary);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetTotalPictureCounts_SingleElement_ReturnsElementPictureCounts()
        {
            // Arrange
            var listDamageSummary = new List<DamageSummary>
        {
            new DamageSummary { PictureCounts = 5 }
        };

            // Act
            int result = AsposeWordsServices.GetTotalPictureCounts(listDamageSummary);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void GetTotalPictureCounts_MultipleElements_ReturnsSumOfElementPictureCounts()
        {
            // Arrange
            var listDamageSummary = new List<DamageSummary>
        {
            new DamageSummary { PictureCounts = 5 },
            new DamageSummary { PictureCounts = 3 },
            new DamageSummary { PictureCounts = 7 }
        };

            // Act
            int result = AsposeWordsServices.GetTotalPictureCounts(listDamageSummary);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void GetSummaryTableBookmarkValue_BridgeDeckBookmarkStartName_ReturnsCorrectValue()
        {
            // Arrange
            var bookmarkStartName = AsposeWordsServices.BridgeDeckBookmarkStartName;

            // Act
            string result = AsposeWordsServices.GetSummaryTableBookmarkValue(bookmarkStartName);

            // Assert
            Assert.Equal($"_Ref{App.TableRefOffset + 1}", result);
        }

        [Fact]
        public void GetSummaryTableBookmarkValue_SuperSpaceBookmarkStartName_ReturnsCorrectValue()
        {
            // Arrange
            var bookmarkStartName = AsposeWordsServices.SuperSpaceBookmarkStartName;

            // Act
            string result = AsposeWordsServices.GetSummaryTableBookmarkValue(bookmarkStartName);

            // Assert
            Assert.Equal($"_Ref{App.TableRefOffset + 2}", result);
        }

        [Fact]
        public void GetSummaryTableBookmarkValue_OtherBookmarkStartName_ReturnsCorrectValue()
        {
            // Arrange
            var bookmarkStartName = "OtherBookmarkStartName";

            // Act
            string result = AsposeWordsServices.GetSummaryTableBookmarkValue(bookmarkStartName);

            // Assert
            Assert.Equal($"_Ref{App.TableRefOffset + 3}", result);
        }
    }
}
