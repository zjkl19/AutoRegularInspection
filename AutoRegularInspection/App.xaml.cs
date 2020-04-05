using System.Windows;
using Ninject;
using AutoRegularInspection.IRepository;
using AutoRegularInspection.Services;

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

        public const string DamageSummaryFileName = "外观检查.xlsx";
        public const string TemplateReportFileName = "外观检查报告模板.docx";
        public const string OutputReportFileName = "自动生成的外观检查报告.docx";


        public App()
        {
            //IOC，依赖注入
        }
    }
}
