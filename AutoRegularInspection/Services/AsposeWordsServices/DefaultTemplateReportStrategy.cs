using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class DefaultTemplateReportStrategy : IReportGenerationStrategy
    {
        public void Generate(AsposeWordsServices service, ProgressBarModel progressModel)
        {
            service.GenerateDefaultTemplateReport(progressModel);
        }
    }
}
