using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace AutoRegularInspection.Services
{
    public static class ImageExtensionSettings
    {
        private const string DefaultExtensions = ".jpg,.jpeg,.png,.bmp,.tif,.tiff,.webp";

        public static string[] GetSupportedExtensions()
        {
            return ParseExtensions(ConfigurationManager.AppSettings["SupportedImageExtensions"], DefaultExtensions);
        }

        public static string[] GetExtensionPriority()
        {
            return ParseExtensions(ConfigurationManager.AppSettings["ImageExtensionPriority"], DefaultExtensions);
        }

        private static string[] ParseExtensions(string raw, string fallback)
        {
            var value = string.IsNullOrWhiteSpace(raw) ? fallback : raw;
            var parts = value.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i].Trim();
                if (part.StartsWith(".", StringComparison.Ordinal))
                {
                    part = part.Substring(1);
                }
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                var lowered = part.ToLowerInvariant();
                if (!list.Contains(lowered))
                {
                    list.Add(lowered);
                }
            }
            return list.ToArray();
        }
    }

    public class ImageLookupResult
    {
        public string SelectedPath { get; set; }
        public List<string> MatchedPaths { get; set; }

        public bool HasConflict
        {
            get { return MatchedPaths != null && MatchedPaths.Count > 1; }
        }
    }

    public static class ImageLocator
    {
        public static ImageLookupResult FindBest(string pictureNo, params string[] folders)
        {
            var extensions = ImageExtensionSettings.GetSupportedExtensions();
            var priority = ImageExtensionSettings.GetExtensionPriority();

            var matches = new List<string>();
            for (int i = 0; i < folders.Length; i++)
            {
                var folder = folders[i];
                if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                {
                    continue;
                }

                for (int j = 0; j < extensions.Length; j++)
                {
                    var pattern = $"*{pictureNo}.{extensions[j]}";
                    matches.AddRange(Directory.GetFiles(folder, pattern));
                }
            }

            var distinct = matches.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            string selected = null;
            for (int i = 0; i < priority.Length; i++)
            {
                var match = distinct.FirstOrDefault(p =>
                    string.Equals(Path.GetExtension(p).TrimStart('.'), priority[i], StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    selected = match;
                    break;
                }
            }

            if (selected == null && distinct.Count > 0)
            {
                selected = distinct[0];
            }

            return new ImageLookupResult
            {
                SelectedPath = selected,
                MatchedPaths = distinct
            };
        }

        public static string BuildConflictWarning(string pictureNo, ImageLookupResult lookup)
        {
            if (lookup == null || lookup.MatchedPaths == null || lookup.MatchedPaths.Count <= 1)
            {
                return null;
            }

            var priority = ImageExtensionSettings.GetExtensionPriority();
            var selectedName = lookup.SelectedPath != null ? Path.GetFileName(lookup.SelectedPath) : "未选取";

            // 如果只是同后缀的重复文件，不提示冲突
            var groups = lookup.MatchedPaths
                .GroupBy(p => Path.GetExtension(p).TrimStart('.').ToLowerInvariant())
                .ToList();
            if (groups.Count <= 1)
            {
                return null;
            }

            var samples = groups.Select(g => Path.GetFileName(g.First())).ToArray();
            return $"编号 {pictureNo} 检测到同名不同后缀：{string.Join("，", samples)}。按优先级 {string.Join(" > ", priority)} 选用 {selectedName}";
        }
    }
}
