using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.IRepository
{
    public interface IReportGenerationStrategy
    {
        void Generate(AsposeWordsServices service, ProgressBarModel progressModel);
    }
}
