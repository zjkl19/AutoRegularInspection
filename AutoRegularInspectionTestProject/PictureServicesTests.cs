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
                new DamageSummary { PictureNo = "1001", PictureCounts = 1, Component = "ComponentA", Damage = "DamageA" }
            };

            var count = PictureServices.ValidatePicturesOfBridgePart(BridgePart.BridgeDeck, list, out List<string> result, tempPic, tempOut, cfg, CancellationToken.None);

            try
            {
                Assert.Equal(0, count);
                Assert.Empty(result);
            }
            finally
            {
                Directory.Delete(tempPic, true);
                Directory.Delete(tempOut, true);
            }
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
                new DamageSummary { PictureNo = "9999", PictureCounts = 1, Component = "ComponentA", Damage = "DamageA" }
            };

            var count = PictureServices.ValidatePicturesOfBridgePart(BridgePart.BridgeDeck, list, out List<string> result, tempPic, tempOut, cfg, CancellationToken.None);

            try
            {
                Assert.Equal(1, count);
                Assert.Single(result);
            }
            finally
            {
                Directory.Delete(tempPic, true);
                Directory.Delete(tempOut, true);
            }
        }

        [Fact]
        public void ValidatePicturesOfBridgePart_RecordsDuplicateWarnings()
        {
            var tempPic = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var tempOut = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempPic);
            Directory.CreateDirectory(tempOut);

            File.WriteAllText(Path.Combine(tempPic, "2732.jpg"), "x");
            File.WriteAllText(Path.Combine(tempPic, "2732.png"), "x");

            var cfg = OptionConfigurationLoader.Load();
            cfg.General.PictureNoSplitSymbol = ";";

            var list = new List<DamageSummary>
            {
                new DamageSummary { PictureNo = "2732", PictureCounts = 1, Component = "ComponentA", Damage = "DamageA" }
            };

            var warnings = new List<string>();
            var count = PictureServices.ValidatePicturesOfBridgePart(BridgePart.BridgeDeck, list, out List<string> result, tempPic, tempOut, cfg, CancellationToken.None, warnings);

            try
            {
                Assert.Equal(0, count);
                Assert.Empty(result);
                Assert.Single(warnings);
                Assert.Contains("2732", warnings[0]);
            }
            finally
            {
                Directory.Delete(tempPic, true);
                Directory.Delete(tempOut, true);
            }
        }
    }
}
