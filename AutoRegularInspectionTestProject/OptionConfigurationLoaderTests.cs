using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.IO;
using Xunit;

namespace AutoRegularInspectionTestProject
{
    public class OptionConfigurationLoaderTests
    {
        [Fact]
        public void Load_ReturnsDefaults_WhenFileIsMissing()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "Option.config");

            var config = OptionConfigurationLoader.Load(tempPath, refresh: true);

            Assert.NotNull(config);
            Assert.NotNull(config.Picture);
            Assert.Equal(79.0, config.Picture.Width);
            Assert.NotNull(config.General);
            Assert.Equal(";", config.General.PictureNoSplitSymbol);
        }

        [Fact]
        public void Load_ReadsValuesFromExistingFile()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, "Option.config");

            var xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <Picture>
    <Width>120.5</Width>
    <Height>60</Height>
    <MaxCompressSize>111</MaxCompressSize>
    <CompressQuality>80</CompressQuality>
    <CompressWidth>800</CompressWidth>
    <CompressHeight>10</CompressHeight>
  </Picture>
  <Bookmark>
    <BridgeDeckBookmarkStartNo>1</BridgeDeckBookmarkStartNo>
    <SuperSpaceBookmarkStartNo>2</SuperSpaceBookmarkStartNo>
    <SubSpaceBookmarkStartNo>3</SubSpaceBookmarkStartNo>
  </Bookmark>
  <BridgeDeckSummaryTable No=""1"" Position=""2"" Component=""3"" Damage=""4"" DamagePosition=""5"" DamageDescription=""6"" PictureNo=""7"" Comment=""8"" />
  <SuperSpaceSummaryTable No=""9"" Position=""10"" Component=""11"" Damage=""12"" DamagePosition=""13"" DamageDescription=""14"" PictureNo=""15"" Comment=""16"" />
  <SubSpaceSummaryTable No=""17"" Position=""18"" Component=""19"" Damage=""20"" DamagePosition=""21"" DamageDescription=""22"" PictureNo=""23"" Comment=""24"" />
  <General>
    <SaveDocxFormat>false</SaveDocxFormat>
    <PictureTableCellWidth>123.4</PictureTableCellWidth>
    <DamageDescriptionInPictureSplitSymbol>#</DamageDescriptionInPictureSplitSymbol>
    <PictureNoSplitSymbol>~</PictureNoSplitSymbol>
    <IntactStructNoInsertSummaryTable>false</IntactStructNoInsertSummaryTable>
    <IntactStructNoInsertSummaryTableString>-</IntactStructNoInsertSummaryTableString>
  </General>
</configuration>";

            File.WriteAllText(filePath, xml);

            var config = OptionConfigurationLoader.Load(filePath, refresh: true);

            Assert.Equal(120.5, config.Picture.Width);
            Assert.Equal(3, config.Bookmark.SubSpaceBookmarkStartNo);
            Assert.Equal(10, config.SuperSpaceSummaryTable.Position);
            Assert.False(config.General.SaveDocxFormat);
            Assert.Equal("~", config.General.PictureNoSplitSymbol);
        }
    }
}
