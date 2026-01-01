using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xunit;

namespace AutoRegularInspectionTestProject
{
    public class PictureServicesTests
    {
        [Fact]
        public void ValidatePicturesOfBridgePart_ReturnsZero_WhenFilesExist()
        {
            var tempPic = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var tempOut = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempPic);
            Directory.CreateDirectory(tempOut);
            File.WriteAllText(Path.Combine(tempPic, "1001.jpg"), "x");

            var cfg = OptionConfigurationLoader.Load();
            cfg.General.PictureNoSplitSymbol = ";";

            var list = new List<DamageSummary>
            {
                new DamageSummary { PictureNo = "1001", PictureCounts = 1, Component = "桥面系", Damage = "裂缝" }
            };

            var count = PictureServices.ValidatePicturesOfBridgePart(BridgePart.BridgeDeck, list, out List<string> result, tempPic, tempOut, cfg, CancellationToken.None);

            Assert.Equal(0, count);
            Assert.Empty(result);
        }

        [Fact]
        public void ValidatePicturesOfBridgePart_ReturnsMissing_WhenFileAbsent()
        {
            var tempPic = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var tempOut = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempPic);
            Directory.CreateDirectory(tempOut);

            var cfg = OptionConfigurationLoader.Load();
            cfg.General.PictureNoSplitSymbol = ";";

            var list = new List<DamageSummary>
            {
                new DamageSummary { PictureNo = "9999", PictureCounts = 1, Component = "桥面系", Damage = "裂缝" }
            };

            var count = PictureServices.ValidatePicturesOfBridgePart(BridgePart.BridgeDeck, list, out List<string> result, tempPic, tempOut, cfg, CancellationToken.None);

            Assert.Equal(1, count);
            Assert.Single(result);
        }
    }
}
