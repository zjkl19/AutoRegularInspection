using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Repository
{
    public class ExcelDataRepository : IDataRepository
    {
        private static bool _damageFileValidated;

        /// <summary>
        /// 读取病害数据
        /// </summary>
        /// <param name="workSheetName">工作簿名称</param>
        /// <returns></returns>
        public List<DamageSummary> ReadDamageData(BridgePart bridgePart, string strFilePath = App.DamageSummaryFileName)
        {
            //string strFilePath = App.DamageSummaryFileName;
            string workSheetName = EnumHelper.GetEnumDesc(bridgePart).ToString();
            var lst = new List<DamageSummary>();

            if (!File.Exists(strFilePath))
            {
                return lst;
            }

            // 仅对默认“外观检查.xlsx”做启动时校验，确保数据可解析
            if (!_damageFileValidated && string.Equals(strFilePath, App.DamageSummaryFileName, StringComparison.OrdinalIgnoreCase))
            {
                ValidateDamageSummaryWorkbook(strFilePath);
                _damageFileValidated = true;
            }

            try
            {

                FileInfo file = new FileInfo(strFilePath);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[workSheetName];
                    int rowCount = GetRowCount(worksheet);

                    //bool validationResult = false;
                    int row = 2;    //excel中行指针
                    //行号不为空，则继续添加
                    //while (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Value.ToString()))
                    for (row = 2; row <= rowCount; row++)
                    {
                        //
                        //1、处理excel数据导入;
                        //2、验证"视图模型";
                        //3、验证业务模型;
                        lst.Add(CreateDamageSummaryFromRow(worksheet, row));

                    }
                }
                //显示导入结果
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从Excel文件中的特定工作表读取损坏数据。
        /// </summary>
        /// <param name="workSheetName">工作表的名称。</param>
        /// <param name="strFilePath">Excel文件的路径。</param>
        /// <returns>包含缺损数据的DamageSummary对象列表。</returns>
        public List<DamageSummary> ReadDamageData(string workSheetName, string strFilePath)
        {
            var lst = new List<DamageSummary>();

            // 检查文件是否存在
            if (!File.Exists(strFilePath))
            {
                return lst;
            }

            try
            {
                FileInfo file = new FileInfo(strFilePath);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // 获取指定名称的工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[workSheetName];
                    int rowCount = GetRowCount(worksheet);

                    // 遍历行并创建DamageSummary对象
                    for (int row = 2; row <= rowCount; row++)
                    {
                        lst.Add(CreateDamageSummaryFromRow(worksheet, row));
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从Excel文件中的所有工作表读取损坏数据。
        /// </summary>
        /// <param name="strFilePath">Excel文件的路径。</param>
        /// <returns>一个字典，键为工作表名称，值为包含损坏数据的DamageSummary对象列表。</returns>
        public Dictionary<string, List<DamageSummary>> ReadAllDamageDataFromFile(string strFilePath)
        {
            var allDamageData = new Dictionary<string, List<DamageSummary>>();

            // 检查文件是否存在
            if (!File.Exists(strFilePath))
            {
                return allDamageData;
            }

            try
            {
                FileInfo file = new FileInfo(strFilePath);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // 遍历工作簿中的每个工作表
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        var damageData = ReadDamageData(worksheet.Name, strFilePath);
                        allDamageData.Add(worksheet.Name, damageData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return allDamageData;
        }


        /// <summary>
        /// 获取工作表中有效数据的行数（跳过表头）
        /// </summary>
        /// <param name="worksheet">Excel工作表</param>
        /// <returns>有效数据的行数</returns>
        private int GetRowCount(ExcelWorksheet worksheet)
        {
            int rowCount = 2;  // 从第2行开始，因为第1行是表头
            while (true)
            {
                try
                {
                    // 如果当前行的第一个单元格为空或仅包含空白，则认为是行尾
                    if (string.IsNullOrWhiteSpace(worksheet.Cells[rowCount + 1, 1].Value?.ToString()))
                    {
                        break;  // 结束循环
                    }
                }
                catch
                {
                    break;  // 如果读取单元格时发生异常，也结束循环
                }
                rowCount++;  // 增加行计数器，处理下一行
            }
            return rowCount;  // 返回有效数据的行数
        }

        private DamageSummary CreateDamageSummaryFromRow(ExcelWorksheet worksheet, int row)
        {
            return new DamageSummary
            {
                No = row - 1,
                Position = GetValue(worksheet, row, "位置"),
                Component = GetValue(worksheet, row, 3),
                Damage = GetValue(worksheet, row, 4),
                DamagePosition = GetValue(worksheet, row, "缺损位置"),
                DamageDescription = GetValue(worksheet, row, "缺损程度"),
                DamageDescriptionInPicture = GetValue(worksheet, row, "图片描述"),
                PictureNo = GetValue(worksheet, row, "照片编号"),
                CustomPictureNo = GetValue(worksheet, row, "自定义照片编号"),
                Comment = GetValue(worksheet, row, "备注"),
                Unit1 = GetValue(worksheet, row, "单位1"),
                Unit1Counts = GetUnit1Counts(GetValue(worksheet, row, "单位1数量")),
                Unit2 = GetValue(worksheet, row, "单位2"),
                Unit2Counts = GetUnit2Counts(GetValue(worksheet, row, "单位2数量")),
                DamagePercentage = GetDamagePercentage(worksheet, row)
            };
        }

        private string GetValue(ExcelWorksheet worksheet, int row, string columnName)
        {
            return worksheet.Cells[row, SaveExcelService.FindColumnIndexByName(worksheet, columnName)].Value?.ToString() ?? string.Empty;
        }

        private string GetValue(ExcelWorksheet worksheet, int row, int columnIndex)
        {
            return worksheet.Cells[row, columnIndex].Value?.ToString() ?? string.Empty;
        }


        private int GetUnit1Counts(string unitCountsString)
        {
            if(string.IsNullOrWhiteSpace(unitCountsString))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(unitCountsString, CultureInfo.InvariantCulture);
            }
        }

        private decimal GetUnit2Counts(string unitCountsString)
        {
            if (string.IsNullOrWhiteSpace(unitCountsString))
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(unitCountsString, CultureInfo.InvariantCulture);
            }
        }

        private decimal GetDamagePercentage(ExcelWorksheet worksheet, int row)
        {
            string value = worksheet.Cells[row, SaveExcelService.FindColumnIndexByName(worksheet, "缺损百分比")].Value?.ToString() ?? "0";
            return decimal.TryParse(value, out decimal result) ? result : 0;
        }

        private void ValidateDamageSummaryWorkbook(string strFilePath)
        {
            var errors = new List<string>();
            var allowedUnit1 = new HashSet<string>(GlobalData.Unit1ComboBox.Select(u => u.DisplayTitle), StringComparer.OrdinalIgnoreCase);
            var allowedUnit2 = new HashSet<string>(GlobalData.Unit2ComboBox.Select(u => u.DisplayTitle), StringComparer.OrdinalIgnoreCase);

            var file = new FileInfo(strFilePath);
            using (var package = new ExcelPackage(file))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    // 仅校验三大部位
                    if (worksheet == null)
                    {
                        continue;
                    }

                    int rowCount = GetRowCount(worksheet);
                    int unit1Col = SaveExcelService.FindColumnIndexByName(worksheet, "单位1数量");
                    int unit2Col = SaveExcelService.FindColumnIndexByName(worksheet, "单位2数量");
                    int unit1NameCol = SaveExcelService.FindColumnIndexByName(worksheet, "单位1");
                    int unit2NameCol = SaveExcelService.FindColumnIndexByName(worksheet, "单位2");

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string unit1CountRaw = worksheet.Cells[row, unit1Col].Value?.ToString() ?? string.Empty;
                        string unit2CountRaw = worksheet.Cells[row, unit2Col].Value?.ToString() ?? string.Empty;
                        string unit1Name = worksheet.Cells[row, unit1NameCol].Value?.ToString() ?? string.Empty;
                        string unit2Name = worksheet.Cells[row, unit2NameCol].Value?.ToString() ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(unit1CountRaw))
                        {
                            decimal parsed;
                            if (!decimal.TryParse(unit1CountRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) || parsed < 0)
                            {
                                errors.Add($"{worksheet.Name} 行{row} 列\"单位1数量\"应为非负数字，实际：{unit1CountRaw}");
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(unit2CountRaw))
                        {
                            decimal parsed;
                            if (!decimal.TryParse(unit2CountRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) || parsed < 0)
                            {
                                errors.Add($"{worksheet.Name} 行{row} 列\"单位2数量\"应为非负数字，实际：{unit2CountRaw}");
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(unit1Name) && !allowedUnit1.Contains(unit1Name.Trim()))
                        {
                            errors.Add($"{worksheet.Name} 行{row} 列\"单位1\"不在统计单位表中：{unit1Name}");
                        }

                        if (!string.IsNullOrWhiteSpace(unit2Name) && !allowedUnit2.Contains(unit2Name.Trim()))
                        {
                            errors.Add($"{worksheet.Name} 行{row} 列\"单位2\"不在统计单位表中：{unit2Name}");
                        }
                    }
                }
            }

            if (errors.Count > 0)
            {
                throw new DataValidationException(errors);
            }
        }
    }
}
