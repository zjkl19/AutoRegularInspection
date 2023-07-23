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
        public static void InitListDamageSummary(List<DamageSummary> listDamageSummary, int firstIndex = 1000000, BridgePart bridgePart = BridgePart.BridgeDeck)
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

                        listDamageSummary[i].DamageValue = componentFound.FirstOrDefault().DamageComboBox.FirstOrDefault(x => x.Title == "其它").Idx;
                    }

                }
                else
                {
                    listDamageSummary[i].DamageComboBox = componentComboBox.FirstOrDefault(x => x.Title == "其它").DamageComboBox;
                    listDamageSummary[i].ComponentValue = componentComboBox.FirstOrDefault(x => x.Title == "其它").Idx;
                }

            }
        }

        private static void SetStatisticsUnitComboBox(List<DamageSummary> listDamageSummary)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                ObservableCollection<StatisticsUnit> unit1ComboBox = GlobalData.Unit1ComboBox;
                ObservableCollection<StatisticsUnit> unit2ComboBox = GlobalData.Unit2ComboBox;
                IEnumerable<StatisticsUnit> unit1Found, unit2Found = null;

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
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OptionConfiguration));
            StreamReader reader = new StreamReader($"{App.ConfigurationFolder}\\{App.ConfigFileName}");    //TODO：找不到文件的判断
            var deserializedConfig = (OptionConfiguration)serializer.Deserialize(reader);

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(listDamageSummary[i].PictureNo))
                {
                    listDamageSummary[i].PictureCounts = 0;
                }
                else
                {
                    listDamageSummary[i].PictureCounts = listDamageSummary[i].PictureNo.Split(deserializedConfig.General.PictureNoSplitSymbol[0]).Length;
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

        public static void GenerateDamageStatisticsTable(ObservableCollection<DamageSummary> bridgeDeckListDamageSummary, ObservableCollection<DamageSummary> superSpaceListDamageSummary, ObservableCollection<DamageSummary> subSpaceListDamageSummary)
        {
            string saveFileName = $"桥梁检测病害统计汇总表.xlsx";
            string tempFileName = $"temp{saveFileName}";

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }

            var file = new FileInfo(tempFileName);

            using (var excelPackage = new ExcelPackage(file))
            {
                // 桥面系
                GenerateWorksheet(excelPackage, "桥面系病害数据", bridgeDeckListDamageSummary, BridgePart.BridgeDeck);
                GenerateSummaryWorksheet(excelPackage, "桥面系病害汇总", bridgeDeckListDamageSummary, BridgePart.BridgeDeck);

                // 上部结构
                GenerateWorksheet(excelPackage, "上部结构病害数据", superSpaceListDamageSummary, BridgePart.SuperSpace);
                GenerateSummaryWorksheet(excelPackage, "上部结构病害汇总", superSpaceListDamageSummary, BridgePart.SuperSpace);

                // 下部结构
                GenerateWorksheet(excelPackage, "下部结构病害数据", subSpaceListDamageSummary, BridgePart.SubSpace);
                GenerateSummaryWorksheet(excelPackage, "下部结构病害汇总", subSpaceListDamageSummary, BridgePart.SubSpace);

                excelPackage.Save();
            }

            File.Copy(tempFileName, saveFileName, true);
            File.Delete(tempFileName);
        }

        // 这个函数用于生成数据工作表
        public static void GenerateWorksheet(ExcelPackage excelPackage, string worksheetName, ObservableCollection<DamageSummary> damageList, BridgePart bridgePart)
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);
            int rowIndex = 2;

            // 添加表头
            worksheet.Cells[1, 1].Value = "序号";
            worksheet.Cells[1, 2].Value = bridgePart == BridgePart.BridgeDeck ? "要素" : "构件类型";
            worksheet.Cells[1, 3].Value = "病害类型";
            worksheet.Cells[1, 4].Value = "单位1";
            worksheet.Cells[1, 5].Value = "单位1数量";
            worksheet.Cells[1, 6].Value = "单位2";
            worksheet.Cells[1, 7].Value = "单位2数量";

            var groupedDamageList = damageList.Where(x => x.GetUnit1() != "无").GroupBy(x => new { ComponentName = x.GetComponentName(bridgePart), DamageName = x.GetDamageName(bridgePart) });

            foreach (var group in groupedDamageList)
            {
                foreach (var damageSummary in group)
                {
                    worksheet.Cells[rowIndex, 1].Value = rowIndex - 1;
                    worksheet.Cells[rowIndex, 2].Value = damageSummary.GetComponentName(bridgePart);
                    worksheet.Cells[rowIndex, 3].Value = damageSummary.GetDamageName(bridgePart);
                    worksheet.Cells[rowIndex, 4].Value = damageSummary.GetDisplayUnit1();
                    worksheet.Cells[rowIndex, 5].Value = damageSummary.Unit1Counts;
                    worksheet.Cells[rowIndex, 6].Value = damageSummary.GetDisplayUnit2();
                    worksheet.Cells[rowIndex, 7].Value = damageSummary.Unit2Counts;
                    rowIndex++;
                }
            }
        }

        public static void GenerateSummaryWorksheet(ExcelPackage excelPackage, string worksheetName, ObservableCollection<DamageSummary> damageList, BridgePart bridgePart)
        {
            var damageStatistics = damageList.Where(x => x.GetUnit1() != "无").GroupBy(x => new { ComponentName = x.GetComponentName(bridgePart), DamageName = x.GetDamageName(bridgePart) });
            var worksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);
            int rowIndex = 2;

            // 添加表头
            worksheet.Cells[1, 1].Value = "序号";
            worksheet.Cells[1, 2].Value = "要素";
            worksheet.Cells[1, 3].Value = "病害类型";
            worksheet.Cells[1, 4].Value = "单位1";
            worksheet.Cells[1, 5].Value = "单位1数量";
            worksheet.Cells[1, 6].Value = "单位2";
            worksheet.Cells[1, 7].Value = "单位2数量";

            int dataRowIndex = 2;  // This will track the row index for the original data in the other worksheet.
            string dataSourceSheetName = string.Empty;

            switch (worksheetName)
            {
                case "桥面系病害汇总":
                    dataSourceSheetName = "桥面系病害数据";
                    break;
                case "上部结构病害汇总":
                    dataSourceSheetName = "上部结构病害数据";
                    break;
                case "下部结构病害汇总":
                    dataSourceSheetName = "下部结构病害数据";
                    break;
            }

            foreach (var v1 in damageStatistics)
            {
                worksheet.Cells[rowIndex, 1].Value = rowIndex - 1;
                worksheet.Cells[rowIndex, 2].Value = v1.Key.ComponentName.ToString(CultureInfo.InvariantCulture);
                worksheet.Cells[rowIndex, 3].Value = v1.Key.DamageName.ToString(CultureInfo.InvariantCulture);
                worksheet.Cells[rowIndex, 4].Value = v1.FirstOrDefault().GetDisplayUnit1();

                // Calculate the start and end rows for the SUM formula based on the count of items in the group.
                var startRow = dataRowIndex;
                var endRow = dataRowIndex + v1.Count() - 1;

                worksheet.Cells[rowIndex, 5].Formula = $"SUM('{dataSourceSheetName}'!E{startRow}:E{endRow})";
                worksheet.Cells[rowIndex, 6].Value = v1.FirstOrDefault().GetDisplayUnit2();
                worksheet.Cells[rowIndex, 7].Formula = $"SUM('{dataSourceSheetName}'!G{startRow}:G{endRow})";

                rowIndex++;

                // Update the data row index for the next group.
                dataRowIndex = endRow + 1;
            }
        }




    }
}