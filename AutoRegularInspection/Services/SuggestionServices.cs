using AutoRegularInspection.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class SuggestionServices
    {
        /// <summary>
        /// 根据不同的桥梁部位，提出建议
        /// </summary>
        /// <param name="listDamageSummary"></param>
        /// <returns></returns>
        public string MakeSuggestions(List<DamageSummary> listDamageSummary)
        {
            string strFilePath = "病害处理建议库.xlsx";
            var workSheetName = "病害处理建议库";

            MatchCollection matches;
            var regexList = new List<Regex>();    //病害描述特征值
            var suggestionList = new List<string>();    //建议

            var suggestions = string.Empty;    //病害处理建议

            if (!File.Exists(strFilePath))
            {
                return string.Empty;
            }

            try
            {
                var file = new FileInfo(strFilePath);

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[workSheetName];
                    int rowCount = 2;// worksheet.Dimension.Rows;   //worksheet.Dimension.Rows指的是所有列中最大行
                    //首行：表头不导入
                    bool rowCur = true;    //行游标指示器
                                           //rowCur=false表示到达行尾
                                           //计算行数
                    while (rowCur)
                    {
                        try
                        {
                            //跳过表头
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[rowCount + 1, 1].Value.ToString()))
                            {
                                rowCur = false;
                            }
                        }
                        catch (Exception)   //读取异常则终止
                        {
                            rowCur = false;
                        }

                        if (rowCur)
                        {
                            rowCount++;
                        }
                    }

                    //bool validationResult = false;
                    int row = 2;    //excel中行指针
                    //行号不为空，则继续添加
                    for (row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            regexList.Add(new Regex($"{ worksheet.Cells[row, 2].Value.ToString() }"));
                            suggestionList.Add(worksheet.Cells[row, 3].Value.ToString());
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                }
                //显示导入结果

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //
            Regex regex;

            for (int i = 0; i < regexList.Count; i++)
            {
                regex = regexList[i];

                for(int j=0;j<listDamageSummary.Count;j++)
                {
                    if(regex.Matches(listDamageSummary[j].Damage).Count>0 || regex.Matches(listDamageSummary[j].DamageDescription).Count > 0)
                    {
                        suggestions+= $"{suggestionList[i]}；\r\n";
                        break;
                    }
                 }
                
            }
            suggestions += "建议管养单位应严格按照现行养护规范要求，对桥梁进行日常的维护和检查工作，严禁超载车辆通行，确保桥梁的完好和安全使用。";
            return suggestions;

        }
    }
}
