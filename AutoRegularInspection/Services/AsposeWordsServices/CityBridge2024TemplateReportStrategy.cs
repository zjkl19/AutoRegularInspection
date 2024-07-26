using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class CityBridge2024TemplateReportStrategy : IReportGenerationStrategy
    {
        public void Generate(AsposeWordsServices service, ProgressBarModel progressModel)
        {
            service.GenerateCityBridge2024TemplateReport(progressModel);
        }
    }
}
