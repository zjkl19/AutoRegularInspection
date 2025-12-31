using AutoRegularInspection.Models;
using System;
using System.IO;

namespace AutoRegularInspection.Services
{
    /// <summary>
    /// 校验模板配置与物理文件/策略映射是否有效。
    /// </summary>
    public static class TemplateValidator
    {
        public static bool Validate(ComboBoxReportTemplates template, out string message)
        {
            return Validate(template, App.ReportTemplatesFolder, out message);
        }

        public static bool Validate(ComboBoxReportTemplates template, string templatesFolder, out string message)
        {
            message = string.Empty;
            if (template == null)
            {
                message = "未选择模板。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(template.Name))
            {
                message = "模板文件名为空，请检查 templates.json。";
                return false;
            }

            var folder = templatesFolder;
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = App.ReportTemplatesFolder;
            }

            var fullPath = Path.Combine(folder, template.Name);
            if (!File.Exists(fullPath))
            {
                message = string.Format("未找到模板文件：{0}", fullPath);
                return false;
            }

            if (!ReportGenerationStrategyFactory.SupportsTemplate(template.Name))
            {
                message = string.Format("当前模板未配置生成策略：{0}", template.Name);
                return false;
            }

            return true;
        }
    }
}
