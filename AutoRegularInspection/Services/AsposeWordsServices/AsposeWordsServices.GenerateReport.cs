
using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public partial class AsposeWordsServices
    {
        public void GenerateReport(ref ProgressBarModel progressModel)
        {
            var strategy = ReportGenerationStrategyFactory.CreateStrategy(_generateReportSettings.ComboBoxReportTemplates.Name);
            strategy.Generate(this, progressModel);
        }
    }
}
