using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.IRepository
{
    public interface IDataRepository
    {
        List<DamageSummary> ReadDamageData(BridgePart bridgePart, string strFilePath = App.DamageSummaryFileName);
    }
}
