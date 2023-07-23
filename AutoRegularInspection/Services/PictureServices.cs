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

        public static int ValidatePicturesOfBridgePart(BridgePart bridgePart, List<DamageSummary> lst, out List<string> validationResult)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OptionConfiguration));
            StreamReader reader = new StreamReader($"{App.ConfigurationFolder}\\{App.ConfigFileName}");    //TODO：找不到文件的判断
            var deserializedConfig = (OptionConfiguration)serializer.Deserialize(reader);

            validationResult = new List<string>();
            int totalCounts = 0;
            string[] dirs, outdirs;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].PictureCounts == 0)    //没有照片，不需要验证
                {
                    continue;
                }
                else if (lst[i].PictureCounts == 1)
                {
                    dirs = Directory.GetFiles($@"{App.PicturesFolder}/", $"*{lst[i].PictureNo}.*");    //结果含有路径
                    outdirs = Directory.GetFiles($@"{App.PicturesOutFolder}/", $"*{lst[i].PictureNo}.*");
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
                        dirs = Directory.GetFiles($@"{App.PicturesFolder}/", $"*{pictures[j]}.*");    //结果含有路径
                        outdirs = Directory.GetFiles($@"{App.PicturesOutFolder}/", $"*{pictures[j]}.*");
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
