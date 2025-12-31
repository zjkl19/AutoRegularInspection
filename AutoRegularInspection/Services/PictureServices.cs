using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public static class PictureServices
    {
        public static int ValidatePictures(List<DamageSummary> l1, List<DamageSummary> l2, List<DamageSummary> l3, out List<string> bridgeDeckValidationResult, out List<string> superSpaceValidationResult, out List<string> subSpaceValidationResult)
        {
            int totalInvalidPictureCounts;
            totalInvalidPictureCounts = ValidatePicturesOfBridgePart(BridgePart.BridgeDeck, l1, out bridgeDeckValidationResult);
            totalInvalidPictureCounts += ValidatePicturesOfBridgePart(BridgePart.SuperSpace, l2, out superSpaceValidationResult);
            totalInvalidPictureCounts += ValidatePicturesOfBridgePart(BridgePart.SubSpace, l3, out subSpaceValidationResult);
            return totalInvalidPictureCounts;
        }

        /// <summary>
        /// 验证图片的方法，处理Dictionary<string, List<DamageSummary>>类型的参数。
        /// </summary>
        /// <param name="allDamageData">包含所有损坏数据的字典。</param>
        /// <param name="validationResults">存储验证结果的字典。</param>
        /// <returns>无效图片的总数。</returns>
        public static int ValidatePictures(Dictionary<string, List<DamageSummary>> allDamageData, out Dictionary<string, List<string>> validationResults)
        {
            int totalInvalidPictureCounts = 0;
            validationResults = new Dictionary<string, List<string>>();

            foreach (var kvp in allDamageData)
            {
                string key = kvp.Key;
                List<DamageSummary> damageSummaryList = kvp.Value;

                int invalidCount = ValidatePicturesOfBridgePart(BridgePart.SuperSpace, damageSummaryList, out List<string> partValidationResult);
                totalInvalidPictureCounts += invalidCount;

                validationResults[key] = partValidationResult;
            }

            return totalInvalidPictureCounts;
        }

        public static int ValidatePicturesOfBridgePart(BridgePart bridgePart, List<DamageSummary> lst, out List<string> validationResult)
        {
            var deserializedConfig = OptionConfigurationLoader.Load();
            return ValidatePicturesOfBridgePart(bridgePart, lst, out validationResult, null, null, deserializedConfig);
        }

        /// <summary>
        /// 提供可覆盖的图片目录与配置，用于测试或自定义路径。
        /// </summary>
        public static int ValidatePicturesOfBridgePart(BridgePart bridgePart, List<DamageSummary> lst, out List<string> validationResult, string picturesFolder, string picturesOutFolder, OptionConfiguration config)
        {
            validationResult = new List<string>();
            int totalCounts = 0;
            var picFolder = string.IsNullOrWhiteSpace(picturesFolder) ? App.PicturesFolder : picturesFolder;
            var outFolder = string.IsNullOrWhiteSpace(picturesOutFolder) ? App.PicturesOutFolder : picturesOutFolder;
            var deserializedConfig = config ?? OptionConfigurationLoader.Load();
            string[] dirs, outdirs;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].PictureCounts == 0)    //没有照片，不需要验证
                {
                    continue;
                }
                else if (lst[i].PictureCounts == 1)
                {
                    dirs = Directory.GetFiles($@"{picFolder}/", $"*{lst[i].PictureNo}.*");    //结果含有路径
                    outdirs = Directory.GetFiles($@"{outFolder}/", $"*{lst[i].PictureNo}.*");
                    if (dirs.Length == 0 && outdirs.Length == 0)
                    {
                        totalCounts++;
                        validationResult.Add($"{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component},{lst[i].Damage}照片{lst[i].PictureNo}不存在");
                        //writer.WriteLine($"{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component},{lst[i].Damage}照片{lst[i].PictureNo}不存在");                            
                    }
                }
                else if (lst[i].PictureCounts >= 2)
                {
                    var pictures = lst[i].PictureNo.Split(deserializedConfig.General.PictureNoSplitSymbol[0]);

                    for (int j = 0; j < pictures.Length; j++)
                    {
                        dirs = Directory.GetFiles($@"{picFolder}/", $"*{pictures[j]}.*");    //结果含有路径
                        outdirs = Directory.GetFiles($@"{outFolder}/", $"*{pictures[j]}.*");
                        if (dirs.Length == 0 && outdirs.Length == 0)
                        {
                            totalCounts++;
                            validationResult.Add($"{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component}照片{pictures[j]}不存在");
                            //writer.WriteLine($"{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component}照片{pictures[j]}不存在");
                        }
                    }
                }
                else    //异常、负数等情况
                {
                    //writer.WriteLine($"未知情况：{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component},{lst[i].Damage}不属于没有照片，纸片只有1张或多张的情况");
                    validationResult.Add($"未知情况：{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component},{lst[i].Damage}不属于没有照片，纸片只有1张或多张的情况");
                }
            }
            return totalCounts;
        }

    }
}
