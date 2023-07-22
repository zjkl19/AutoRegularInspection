using AutoRegularInspection.IRepository;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class ReportGenerationStrategyFactory
    {
        private static readonly Dictionary<string, IReportGenerationStrategy> strategies = new Dictionary<string, IReportGenerationStrategy>
        {
            { "建研-常规定检--晋安区桥梁模板.doc", new Jinan2023TemplateReportStrategy() },
            { "交通综合评价报告模板.docx", new TransportationTemplateReportStrategy() },
            { "自定义外观检查报告模板.docx", new DefaultTemplateReportStrategy() }
            //... 其他模板策略
        };

        public static IReportGenerationStrategy CreateStrategy(string templateName)
        {
            if (strategies.TryGetValue(templateName, out var strategy))
            {
                return strategy;
            }
            throw new Exception("不支持该模板");
        }
    }
}
