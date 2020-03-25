using AutoRegularInspection.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class SaveExcelService
    {
        /// <summary>
        /// 新建一个临时excel，然后把原来excel覆盖掉，最后删掉原来的临时文件
        /// </summary>
        /// <param name="listDamageSummary"></param>
        /// <returns>1表示返回成功，0表示返回失败</returns>
        public static int SaveExcel(List<DamageSummary> bridgeDeckListDamageSummary
            , List<DamageSummary> superSpaceListDamageSummary
            , List<DamageSummary> subSpaceListDamageSummary)
        {

            string sheetName = string.Empty;

            var file = new FileInfo("temp外观检查.xlsx");

            try
            {

                List<int> failRows = new List<int>();    //excel中写入失败的行数

                using (var excelPackage = new ExcelPackage(file))
                {
                    // 添加worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("桥面系");
                    //添加表头
                    worksheet.Cells[1, 1].Value = "序号";
                    worksheet.Cells[1, 2].Value = "位置";
                    worksheet.Cells[1, 3].Value = "要素";
                    worksheet.Cells[1, 4].Value = "缺损类型";
                    worksheet.Cells[1, 5].Value = "缺损描述";
                    worksheet.Cells[1, 6].Value = "图片描述";
                    worksheet.Cells[1, 7].Value = "照片编号";

                    //添加值
                    for (int i = 0; i < bridgeDeckListDamageSummary.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = i + 1;
                        worksheet.Cells[i + 2, 2].Value = bridgeDeckListDamageSummary[i].Position;
                        worksheet.Cells[i + 2, 3].Value = bridgeDeckListDamageSummary[i].Component;
                        worksheet.Cells[i + 2, 4].Value = bridgeDeckListDamageSummary[i].Damage;
                        worksheet.Cells[i + 2, 5].Value = bridgeDeckListDamageSummary[i].DamageDescription;
                        worksheet.Cells[i + 2, 6].Value = bridgeDeckListDamageSummary[i].DamageDescriptionInPicture;
                        worksheet.Cells[i + 2, 7].Value = bridgeDeckListDamageSummary[i].PictureNo;
                    }

                    worksheet = excelPackage.Workbook.Worksheets.Add("上部结构");
                    //添加表头
                    worksheet.Cells[1, 1].Value = "序号";
                    worksheet.Cells[1, 2].Value = "位置";
                    worksheet.Cells[1, 3].Value = "构件类型";
                    worksheet.Cells[1, 4].Value = "缺损类型";
                    worksheet.Cells[1, 5].Value = "缺损描述";
                    worksheet.Cells[1, 6].Value = "图片描述";
                    worksheet.Cells[1, 7].Value = "照片编号";

                    //添加值
                    for (int i = 0; i < bridgeDeckListDamageSummary.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = i + 1;
                        worksheet.Cells[i + 2, 2].Value = superSpaceListDamageSummary[i].Position;
                        worksheet.Cells[i + 2, 3].Value = superSpaceListDamageSummary[i].Component;
                        worksheet.Cells[i + 2, 4].Value = superSpaceListDamageSummary[i].Damage;
                        worksheet.Cells[i + 2, 5].Value = superSpaceListDamageSummary[i].DamageDescription;
                        worksheet.Cells[i + 2, 6].Value = superSpaceListDamageSummary[i].DamageDescriptionInPicture;
                        worksheet.Cells[i + 2, 7].Value = superSpaceListDamageSummary[i].PictureNo;
                    }

                    excelPackage.Save();
                }
                return 1;
            }
            catch (Exception ex)
            {
                Debug.Print($"保存excel出错，错误信息：{ex.Message}");
                return 0;
            }

        }
    }
}
