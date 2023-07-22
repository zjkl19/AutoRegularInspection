using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class TransportationTemplateReportStrategy : IReportGenerationStrategy
    {
        public void Generate(AsposeWordsServices service, ProgressBarModel progressModel)
        {
            service.GenerateTransportationTemplateReport(progressModel);
        }
    }
}
