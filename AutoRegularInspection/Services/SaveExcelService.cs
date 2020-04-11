using AutoRegularInspection.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public static class SaveExcelService
    {
        /// <summary>
        /// 新建一个临时excel，然后把原来excel覆盖掉，最后删掉原来的临时文件
        /// </summary>
        /// <param name="listDamageSummary"></param>
        /// <returns>1表示返回成功，0表示返回失败</returns>
        public static int SaveExcel(List<DamageSummary> bridgeDeckListDamageSummary
            , List<DamageSummary> superSpaceListDamageSummary
            , List<DamageSummary> subSpaceListDamageSummary,string saveFileName=App.DamageSummaryFileName)
        {
            string tempFileName = $"temp{App.DamageSummaryFileName}";

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }

            string sheetName = string.Empty;

            var file = new FileInfo(tempFileName);

            try
            {

                List<int> failRows = new List<int>();    //excel中写入失败的行数

                using (var excelPackage = new ExcelPackage(file))
                {
                    // 添加worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("桥面系");
                    InsertVar(bridgeDeckListDamageSummary, worksheet,GlobalData.ComponentComboBox,BridgePart.BridgeDeck);
                    worksheet = excelPackage.Workbook.Worksheets.Add("上部结构");
                    InsertVar(superSpaceListDamageSummary, worksheet, GlobalData.SuperSpaceComponentComboBox, BridgePart.SuperSpace);
                    worksheet = excelPackage.Workbook.Worksheets.Add("下部结构");
                    InsertVar(subSpaceListDamageSummary, worksheet, GlobalData.SubSpaceComponentComboBox, BridgePart.SubSpace);

                    excelPackage.Save();
                }
                File.Copy(tempFileName, saveFileName, true);
                File.Delete(tempFileName);
                return 1;
            }
            catch (Exception ex)
            {
                Debug.Print($"保存excel出错，错误信息：{ex.Message}");
                return 0;
            }

        }

        private static void InsertVar(List<DamageSummary> listDamageSummary, ExcelWorksheet worksheet, ObservableCollection<BridgeDamage> componentComboBox, BridgePart bridgePart)
        {

            //添加表头
            worksheet.Cells[1, 1].Value = "序号";
            worksheet.Cells[1, 2].Value = "位置";
            if (bridgePart == BridgePart.BridgeDeck)
            {
                worksheet.Cells[1, 3].Value = "要素";
            }
            else
            {
                worksheet.Cells[1, 3].Value = "构件类型";
            }
            worksheet.Cells[1, 4].Value = "缺损类型";
            worksheet.Cells[1, 5].Value = "缺损描述";
            worksheet.Cells[1, 6].Value = "图片描述";
            worksheet.Cells[1, 7].Value = "照片编号";
            worksheet.Cells[1, 8].Value = "备注";
            worksheet.Cells[1, 9].Value = "单位1";
            worksheet.Cells[1, 10].Value = "单位1数量";
            worksheet.Cells[1, 11].Value = "单位2";
            worksheet.Cells[1, 12].Value = "单位2数量";
            worksheet.Cells[1, 13].Value = "使用自定义单位";

            //添加值
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = listDamageSummary[i].Position;
                if (componentComboBox[listDamageSummary[i].ComponentValue].Title != "其它")
                {
                    worksheet.Cells[i + 2, 3].Value = componentComboBox[listDamageSummary[i].ComponentValue].Title;
                }
                else    //TODO:考虑"其它"输入为空的情况
                {
                    worksheet.Cells[i + 2, 3].Value = listDamageSummary[i].GetComponentName();
                }

                if (componentComboBox[listDamageSummary[i].ComponentValue].DamageComboBox[listDamageSummary[i].DamageValue].Title != "其它")
                {
                    worksheet.Cells[i + 2, 4].Value = componentComboBox[listDamageSummary[i].ComponentValue].DamageComboBox[listDamageSummary[i].DamageValue].Title;
                }
                else    //TODO:考虑"其它"输入为空的情况
                {
                    worksheet.Cells[i + 2, 4].Value = listDamageSummary[i].Damage;
                }

                worksheet.Cells[i + 2, 5].Value = listDamageSummary[i].DamageDescription;
                worksheet.Cells[i + 2, 6].Value = listDamageSummary[i].DamageDescriptionInPicture;
                worksheet.Cells[i + 2, 7].Value = listDamageSummary[i].PictureNo;
                worksheet.Cells[i + 2, FindColumnIndexByName(worksheet, "备注")].Value = listDamageSummary[i].Comment;
                worksheet.Cells[i + 2, FindColumnIndexByName(worksheet, "单位1")].Value = listDamageSummary[i].GetDisplayUnit1();
                worksheet.Cells[i + 2, FindColumnIndexByName(worksheet, "单位1数量")].Value = listDamageSummary[i].Unit1Counts;
                worksheet.Cells[i + 2, FindColumnIndexByName(worksheet, "单位2")].Value = listDamageSummary[i].GetDisplayUnit2();
                worksheet.Cells[i + 2, FindColumnIndexByName(worksheet, "单位2数量")].Value = listDamageSummary[i].Unit2Counts;
            }
        }

        /// <summary>
        /// 通过名称查找列索引
        /// </summary>
        /// <param name="workSheet">工作簿项目URL:https://epplussoftware.com/<
        /// <param name="keyWord">查找关键字
        /// <param name="maxSearchColumnCounts">最大查找列数，默认为100</param>
        /// <returns>找到则返回正确的列数（索引从1开始），否则返回0</returns>
        public static int FindColumnIndexByName(ExcelWorksheet workSheet,string keyWord,int maxSearchColumnCounts=100)
        {
            for(int i=1;i<maxSearchColumnCounts;i++)
            {
                if((workSheet.Cells[1, i].Value?.ToString() ?? string.Empty)  == keyWord)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
