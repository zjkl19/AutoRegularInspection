using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System.Collections.Generic;
using Xunit;

namespace AutoRegularInspectionTestProject
{
    public class DamageSummaryServicesTests
    {
        [Fact]
        public void InitListDamageSummary1_SetsPictureCountsAndBookmarks()
        {
            var list = new List<DamageSummary>
            {
                new DamageSummary { PictureNo = "1001;1002" },
                new DamageSummary { PictureNo = "2001" },
                new DamageSummary { PictureNo = string.Empty }
            };

            DamageSummaryServices.InitListDamageSummary1(list, 1_000_000);

            Assert.Equal(2, list[0].PictureCounts);
            Assert.Equal("_Ref1000000", list[0].FirstPictureBookmark);
            Assert.Equal("_Ref1000001", list[0].LastPictureBookmark);

            Assert.Equal(1, list[1].PictureCounts);
            Assert.Equal("_Ref1000002", list[1].FirstPictureBookmark);
            Assert.Equal("_Ref1000002", list[1].LastPictureBookmark);

            Assert.Equal(0, list[2].PictureCounts);
            Assert.Equal("_Ref1000003", list[2].FirstPictureBookmark);
            // 当 PictureCounts 为 0 时 LastPictureBookmark 仍然向前推一个，这里只验证未抛异常
        }
    }
}
