using AutoRegularInspection.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OfficeOpenXml;
using System.Globalization;

namespace AutoRegularInspection.Services
{
    /// <summary>
    /// 对List<DamageSummary>数据进行初始化、预处理
    /// </summary>
    public static class DamageSummaryServices
    {
        public static void InitListDamageSummary1(List<DamageSummary> listDamageSummary, int firstIndex = 1000000)
        {
            SetPictureCounts(listDamageSummary);
            SetFirstAndLastPictureBookmark(listDamageSummary, firstIndex);
        }

        /// <summary>
        /// 初始化病害汇总列表
        /// </summary>
        /// <param name="listDamageSummary"></param>
        /// <param name="firstIndex"></param>
        public static void InitListDamageSummary(List<DamageSummary> listDamageSummary, int firstIndex = 1000000,BridgePart bridgePart=BridgePart.BridgeDeck)
        {
            SetPictureCounts(listDamageSummary);
            SetFirstAndLastPictureBookmark(listDamageSummary, firstIndex);
            SetComboBox(listDamageSummary, bridgePart);
            SetStatisticsUnitComboBox(listDamageSummary);
            //for (int i = 0; i < listDamageSummary.Count; i++)
            //{
            //    var img = System.Drawing.Image.FromFile($"PicturesOut/DSC00855.jpg");
            //    var map = new System.Drawing.Bitmap(img);
            //    listDamageSummary[i].PicturePreview= ConvertBitmap(map);

            //}
        }

        private static void SetComboBox(List<DamageSummary> listDamageSummary, BridgePart bridgePart = BridgePart.BridgeDeck)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                //TODO：写单元测试
                //创建映射

                ObservableCollection<BridgeDamage> componentComboBox = null;
                IEnumerable<BridgeDamage> componentFound = null;

                if (bridgePart == BridgePart.BridgeDeck)
                {
                    componentComboBox = GlobalData.ComponentComboBox;
                }
                else if (bridgePart == BridgePart.SuperSpace)
                {
                    componentComboBox = GlobalData.SuperSpaceComponentComboBox;
                }
                else
                {
                    componentComboBox = GlobalData.SubSpaceComponentComboBox;
                }

                componentFound = componentComboBox.Where(x => x.Title == listDamageSummary[i].Component);             

                IEnumerable<BridgeDamage> damageFound = null;

                if (componentFound.Any())
                {
                    listDamageSummary[i].ComponentValue = componentFound.FirstOrDefault().Idx;

                    damageFound = componentFound.FirstOrDefault().DamageComboBox.Where(x => x.Title == listDamageSummary[i].Damage);

                    if (damageFound.Any())
                    {
                        listDamageSummary[i].DamageComboBox = componentFound.FirstOrDefault().DamageComboBox;

                        listDamageSummary[i].DamageValue = damageFound.FirstOrDefault().Idx;
                    }
                    else
                    {
                        listDamageSummary[i].DamageComboBox = componentFound.FirstOrDefault().DamageComboBox;

                        listDamageSummary[i].DamageValue = componentFound.FirstOrDefault().DamageComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx;
                    }

                }
                else
                {
                    listDamageSummary[i].DamageComboBox = componentComboBox.Where(x => x.Title == "其它").FirstOrDefault().DamageComboBox;
                    listDamageSummary[i].ComponentValue = componentComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx;
                }

            }
        }

        private static void SetStatisticsUnitComboBox(List<DamageSummary> listDamageSummary)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                ObservableCollection<StatisticsUnit> unit1ComboBox = GlobalData.Unit1ComboBox; 
                ObservableCollection<StatisticsUnit> unit2ComboBox = GlobalData.Unit2ComboBox;
                IEnumerable<StatisticsUnit> unit1Found,unit2Found = null;

                unit1Found = unit1ComboBox.Where(x => x.DisplayTitle == listDamageSummary[i].Unit1);
                unit2Found = unit2ComboBox.Where(x => x.DisplayTitle == listDamageSummary[i].Unit2);

                if (unit1Found.Any())
                {
                    listDamageSummary[i].Unit1Value = unit1Found.FirstOrDefault().Idx;
                }
                else
                {
                    listDamageSummary[i].Unit1Value = unit1ComboBox.Where(x => x.DisplayTitle == "无").FirstOrDefault().Idx;
                }

                if (unit2Found.Any())
                {
                    listDamageSummary[i].Unit2Value = unit2Found.FirstOrDefault().Idx;
                }
                else
                {
                    listDamageSummary[i].Unit2Value = unit2ComboBox.Where(x => x.DisplayTitle == "无").FirstOrDefault().Idx;
                }

            }
        }

        private static void SetPictureCounts(List<DamageSummary> listDamageSummary)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                if(string.IsNullOrWhiteSpace(listDamageSummary[i].PictureNo))
                {
                    listDamageSummary[i].PictureCounts = 0;
                }
                else
                {
                    listDamageSummary[i].PictureCounts = listDamageSummary[i].PictureNo.Split(',').Count();
                }
            }
        }
        /// <summary>
        /// 要考虑PirctureCounts为0的情况
        /// </summary>
        /// <param name="listDamageSummary"></param>
        private static void SetFirstAndLastPictureBookmark(List<DamageSummary> listDamageSummary, int firstIndex = 1000000)
        {

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                if (i == 0)
                {
                    listDamageSummary[i].FirstPictureBookmarkIndex = firstIndex;
                }
                else
                {
                    listDamageSummary[i].FirstPictureBookmarkIndex = listDamageSummary[i - 1].FirstPictureBookmarkIndex + listDamageSummary[i - 1].PictureCounts;
                }
                listDamageSummary[i].FirstPictureBookmark = $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex}";
                listDamageSummary[i].LastPictureBookmark = $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + listDamageSummary[i].PictureCounts - 1}";
            }
        }

        public static void GenerateDamageStatisticsTable(ObservableCollection<DamageSummary> bridgeDeckListDamageSummary)
        {
            var damageStatistics = bridgeDeckListDamageSummary.GroupBy(x => new { ComponentName = x.GetComponentName(), DamageName = x.GetDamageName() });
            string saveFileName = $"桥梁检测病害统计汇总表.xlsx";
            string tempFileName = $"temp{saveFileName}";
            string sheetName = string.Empty;
            var file = new FileInfo(tempFileName);

            int rowIndex = 2;

            using (var excelPackage = new ExcelPackage(file))
            {
                // 添加worksheet
                var worksheet = excelPackage.Workbook.Worksheets.Add("桥面系病害统计汇总表");
                //添加表头
                worksheet.Cells[1, 1].Value = "序号";
                worksheet.Cells[1, 2].Value = "要素";
                worksheet.Cells[1, 3].Value = "病害类型";
                worksheet.Cells[1, 4].Value = "单位1";
                worksheet.Cells[1, 5].Value = "单位1数量";
                worksheet.Cells[1, 6].Value = "单位2";
                worksheet.Cells[1, 7].Value = "单位2数量";

                foreach (var v1 in damageStatistics)
                {
                    worksheet.Cells[rowIndex, 1].Value = rowIndex - 1;
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "要素")].Value = $"{v1.Key.ComponentName.ToString(CultureInfo.InvariantCulture)}";
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "病害类型")].Value = $"{v1.Key.DamageName.ToString(CultureInfo.InvariantCulture)}";
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "单位1")].Value = $"{v1.FirstOrDefault().GetDisplayUnit1()}";
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "单位1数量")].Value = $"{v1.Sum(x => x.Unit1Counts)}";
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "单位2")].Value = $"{v1.FirstOrDefault().GetDisplayUnit2()}";
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "单位2数量")].Value = $"{v1.Sum(x => x.Unit2Counts)}";
                    rowIndex++;
                }

                worksheet = excelPackage.Workbook.Worksheets.Add("上部结构");
                excelPackage.Save();
            }
            File.Copy(tempFileName, saveFileName, true);
            File.Delete(tempFileName);
        }

        public static System.Windows.Media.Imaging.BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new System.Windows.Media.Imaging.BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}