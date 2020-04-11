using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void GenerateDamageStatisticsTable_Click(object sender, RoutedEventArgs e)
        {

            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;

            try
            {
                GenerateDamageStatisticsTable(_bridgeDeckListDamageSummary);
            }
            catch (Exception ex)
            {
                Debug.Print($"保存excel出错，错误信息：{ex.Message}");
            }

        }

        private static void GenerateDamageStatisticsTable(ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary)
        {
            var damageStatistics = _bridgeDeckListDamageSummary.GroupBy(x => new { ComponentName = x.GetComponentName(), DamageName = x.GetDamageName() });
            string saveFileName = $"桥梁检测病害统计汇总表.xlsx";
            string tempFileName = $"temp{saveFileName}";
            string sheetName = string.Empty;
            var file = new FileInfo(tempFileName);

            int rowIndex = 2;

            using (var excelPackage = new ExcelPackage(file))
            {
                // 添加worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("桥面系病害统计汇总表");
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
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "单位1数量")].Value = $"{v1.Sum(x => x.Unit1Counts)}";
                    worksheet.Cells[rowIndex, SaveExcelService.FindColumnIndexByName(worksheet, "单位2数量")].Value = $"{v1.Sum(x => x.Unit2Counts)}";
                    rowIndex++;

                }

                worksheet = excelPackage.Workbook.Worksheets.Add("上部结构");
                excelPackage.Save();
            }
            File.Copy(tempFileName, saveFileName, true);
            File.Delete(tempFileName);
        }
    }
}
