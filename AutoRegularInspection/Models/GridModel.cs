using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class GridModel
    {
        public GridModel()
        {
            GridData = new ObservableCollection<DamageSummary>();
        }
        public ObservableCollection<DamageSummary> GridData { get; set; }
    }
}
