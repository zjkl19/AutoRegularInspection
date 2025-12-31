using AutoRegularInspection.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace AutoRegularInspection.Services
{
    /// <summary>
    /// 负责加载、缓存 Option.config，并在文件缺失或格式错误时提供带默认值的配置。
    /// </summary>
    public static class OptionConfigurationLoader
    {
        private static readonly object _syncRoot = new object();
        private static OptionConfiguration _cached;
        private static string _cachedPath;

        public static OptionConfiguration Load(string configPath = null, bool refresh = false)
        {
            lock (_syncRoot)
            {
                var path = ResolvePath(configPath);
                if (!refresh && _cached != null && string.Equals(path, _cachedPath, StringComparison.OrdinalIgnoreCase))
                {
                    return _cached;
                }

                _cached = LoadInternal(path);
                _cachedPath = path;
                return _cached;
            }
        }

        public static void SetCache(OptionConfiguration configuration, string configPath = null)
        {
            if (configuration == null)
            {
                return;
            }

            lock (_syncRoot)
            {
                _cached = EnsureDefaults(configuration);
                _cachedPath = ResolvePath(configPath);
            }
        }

        private static string ResolvePath(string configPath)
        {
            if (!string.IsNullOrWhiteSpace(configPath))
            {
                return configPath;
            }

            return Path.Combine(App.ConfigurationFolder, App.ConfigFileName);
        }

        private static OptionConfiguration LoadInternal(string path)
        {
            if (!File.Exists(path))
            {
                return CreateDefault();
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    var serializer = new XmlSerializer(typeof(OptionConfiguration));
                    var config = serializer.Deserialize(stream) as OptionConfiguration;
                    return EnsureDefaults(config);
                }
            }
            catch
            {
                return CreateDefault();
            }
        }

        private static OptionConfiguration EnsureDefaults(OptionConfiguration configuration)
        {
            if (configuration == null)
            {
                configuration = new OptionConfiguration();
            }

            if (configuration.Picture == null)
            {
                configuration.Picture = new Picture();
            }

            if (configuration.Bookmark == null)
            {
                configuration.Bookmark = new Bookmark();
            }

            if (configuration.BridgeDeckSummaryTable == null)
            {
                configuration.BridgeDeckSummaryTable = new BridgeDeckSummaryTable();
            }

            if (configuration.SuperSpaceSummaryTable == null)
            {
                configuration.SuperSpaceSummaryTable = new SuperSpaceSummaryTable();
            }

            if (configuration.SubSpaceSummaryTable == null)
            {
                configuration.SubSpaceSummaryTable = new SubSpaceSummaryTable();
            }

            if (configuration.General == null)
            {
                configuration.General = new General();
            }

            if (configuration.Picture.Width == 0 && configuration.Picture.Height == 0)
            {
                configuration.Picture.Width = 79.0;
                configuration.Picture.Height = 59.4;
                configuration.Picture.MaxCompressSize = 300;
                configuration.Picture.CompressQuality = 90;
                configuration.Picture.CompressWidth = 940.5;
                configuration.Picture.CompressHeight = 0;
            }

            if (configuration.Bookmark.BridgeDeckBookmarkStartNo == 0 &&
                configuration.Bookmark.SuperSpaceBookmarkStartNo == 0 &&
                configuration.Bookmark.SubSpaceBookmarkStartNo == 0)
            {
                configuration.Bookmark.BridgeDeckBookmarkStartNo = 1_000_000;
                configuration.Bookmark.SuperSpaceBookmarkStartNo = 2_000_000;
                configuration.Bookmark.SubSpaceBookmarkStartNo = 3_000_000;
            }

            FillSummaryDefaults(configuration.BridgeDeckSummaryTable, 12.5, 18.1, 22.8, 28.1, 41.5, 41.5, 19.9, 25.0);
            FillSummaryDefaults(configuration.SuperSpaceSummaryTable, 11.5, 18.1, 18.7, 18.9, 38.5, 38.5, 21.9, 25.0);
            FillSummaryDefaults(configuration.SubSpaceSummaryTable, 11.5, 18.1, 18.7, 18.9, 38.5, 38.5, 21.9, 25.0);

            if (configuration.General.PictureTableCellWidth == 0)
            {
                configuration.General.SaveDocxFormat = true;
                configuration.General.PictureTableCellWidth = 194.0;
                configuration.General.DamageDescriptionInPictureSplitSymbol = "$";
                configuration.General.PictureNoSplitSymbol = ";";
                configuration.General.IntactStructNoInsertSummaryTable = true;
                configuration.General.IntactStructNoInsertSummaryTableString = "/";
            }

            return configuration;
        }

        private static void FillSummaryDefaults(BridgeDeckSummaryTable table, double no, double position, double component, double damage, double damagePosition, double damageDescription, double pictureNo, double comment)
        {
            if (table.No == 0 &&
                table.Position == 0 &&
                table.Component == 0 &&
                table.Damage == 0 &&
                table.DamagePosition == 0 &&
                table.DamageDescription == 0 &&
                table.PictureNo == 0 &&
                table.Comment == 0)
            {
                table.No = no;
                table.Position = position;
                table.Component = component;
                table.Damage = damage;
                table.DamagePosition = damagePosition;
                table.DamageDescription = damageDescription;
                table.PictureNo = pictureNo;
                table.Comment = comment;
            }
        }

        private static OptionConfiguration CreateDefault()
        {
            var configuration = new OptionConfiguration
            {
                Picture = new Picture
                {
                    Width = 79.0,
                    Height = 59.4,
                    MaxCompressSize = 300,
                    CompressQuality = 90,
                    CompressWidth = 940.5,
                    CompressHeight = 0
                },
                Bookmark = new Bookmark
                {
                    BridgeDeckBookmarkStartNo = 1_000_000,
                    SuperSpaceBookmarkStartNo = 2_000_000,
                    SubSpaceBookmarkStartNo = 3_000_000
                },
                BridgeDeckSummaryTable = new BridgeDeckSummaryTable
                {
                    No = 12.5,
                    Position = 18.1,
                    Component = 22.8,
                    Damage = 28.1,
                    DamagePosition = 41.5,
                    DamageDescription = 41.5,
                    PictureNo = 19.9,
                    Comment = 25.0
                },
                SuperSpaceSummaryTable = new SuperSpaceSummaryTable
                {
                    No = 11.5,
                    Position = 18.1,
                    Component = 18.7,
                    Damage = 18.9,
                    DamagePosition = 38.5,
                    DamageDescription = 38.5,
                    PictureNo = 21.9,
                    Comment = 25.0
                },
                SubSpaceSummaryTable = new SubSpaceSummaryTable
                {
                    No = 11.5,
                    Position = 18.1,
                    Component = 18.7,
                    Damage = 18.9,
                    DamagePosition = 38.5,
                    DamageDescription = 38.5,
                    PictureNo = 21.9,
                    Comment = 25.0
                },
                General = new General
                {
                    SaveDocxFormat = true,
                    PictureTableCellWidth = 194.0,
                    DamageDescriptionInPictureSplitSymbol = "$",
                    PictureNoSplitSymbol = ";",
                    IntactStructNoInsertSummaryTable = true,
                    IntactStructNoInsertSummaryTableString = "/"
                }
            };

            return configuration;
        }
    }
}
