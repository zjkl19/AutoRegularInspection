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

        public const string ErrorPicName = "ErrorPic.jpg";

        public const string DamageSummaryLibFileName = "桥梁病害汇总表.xlsx";
        public const string ConfigurationFolder = "配置";    //配置文件夹
        public const string DamageProcessingSuggestionsFile = "病害处理建议库.xlsx";
        public const string ReportTemplatesFolder = "报告模板";
        public const string PicturesFolder = "Pictures";
        public const string PicturesOutFolder = "PicturesOut";
        public const string InvalidPicturesStoreFile = "无效照片列表.txt";
        
        public const string DamageSummaryFileName = "外观检查.xlsx";
        public const string DamageSummaryStatisticsFileName = "外观检查 - 统计.xlsx";

        public const string StatisticsUnitFileName = "统计单位表.xlsx";
        public const string TemplateReportFileName = "外观检查报告模板.docx";
        public const string OutputReportFileName = "自动生成的外观检查报告.docx";
        public const string OutputDamageStatisticsFileName = "桥梁检测病害统计汇总表.xlsx";
        public const string ConfigFileName = "Option.config";
        public const char PictureNoSplitSymbol = ';';    //照片编号分隔符号
        public const int TablePictureWidth = 550;    //表格图片一栏默认宽度

        public static List<ComboBoxReportTemplates> TemplateFileList => new List<ComboBoxReportTemplates> {
            new ComboBoxReportTemplates{DisplayName= "交通综合评价报告模板",Name="交通综合评价报告模板.docx",DocStyleOfMainText="迪南交通报告正文",DocStyleOfTable="迪南交通报告表格",DocStyleOfPicture="迪南交通报告图片"}
            ,new ComboBoxReportTemplates{DisplayName= "建研报告模板",Name="外观检查报告模板.docx",DocStyleOfMainText="迪南自动报告正文",DocStyleOfTable="迪南自动报告表格",DocStyleOfPicture="迪南自动报告图片"}
            ,new ComboBoxReportTemplates{DisplayName= "检测中心报告模板",Name="检测中心外观检查报告模板.docx",DocStyleOfMainText="迪南自动报告正文",DocStyleOfTable="迪南自动报告表格",DocStyleOfPicture="迪南自动报告图片"}
            ,new ComboBoxReportTemplates{DisplayName= "自定义报告模板",Name="自定义外观检查报告模板.docx",DocStyleOfMainText="迪南自动报告正文",DocStyleOfTable="迪南自动报告表格",DocStyleOfPicture="迪南自动报告图片"}};

        public App()
        {
            //IOC，依赖注入
        }


    }
}
