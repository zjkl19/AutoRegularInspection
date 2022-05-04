using System.Windows;
using Ninject;
using AutoRegularInspection.IRepository;
using AutoRegularInspection.Services;
using System.Collections.Generic;
using AutoRegularInspection.Models;

namespace AutoRegularInspection
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //全局常量

        //参考https://www.cnblogs.com/Gildor/archive/2010/06/29/1767156.html
        public static double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        public static double ScreenHeight = SystemParameters.PrimaryScreenHeight;

        public const string DocStyleOfMainText = "迪南自动报告正文";
        public const string DocStyleOfTable = "迪南自动报告表格";
        public const string DocStyleOfPicture = "迪南自动报告图片";

        public const string PicturesFolder = "Pictures";
        public const string PicturesOutFolder = "PicturesOut";
        public const string InvalidPicturesStoreFile= "无效照片列表.txt";
        public const string DamageSummaryFileName = "外观检查.xlsx";
        public const string StatisticsUnitFileName = "统计单位表.xlsx";
        public const string TemplateReportFileName = "外观检查报告模板.docx";
        public const string OutputReportFileName = "自动生成的外观检查报告.docx";
        public const string OutputDamageStatisticsFileName = "桥梁检测病害统计汇总表.xlsx";
        public const string ConfigFileName = "Option.config";
        public const char PictureNoSplitSymbol = ';';    //照片编号分隔符号
        public const int TablePictureWidth = 550;    //表格图片一栏默认宽度

        public static List<ComboBoxReportTemplates> TemplateFileList => new List<ComboBoxReportTemplates> { 
            new ComboBoxReportTemplates{DisplayName= "建研报告模板",Name="外观检查报告模板.docx"}
            ,new ComboBoxReportTemplates{DisplayName= "检测中心报告模板",Name="检测中心外观检查报告模板.docx"}};

        public App()
        {
            //IOC，依赖注入
        }
    }
}
