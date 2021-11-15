using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace AutoRegularInspection.Models
{
    public class OptionReportGeneralSettings : UpdatePropertyAndOnPropertyChangedBase, INotifyPropertyChanged
    {
        private bool _IntactStructNoInsertSummaryTable;
        public bool IntactStructNoInsertSummaryTable
        {
            get => _IntactStructNoInsertSummaryTable;
            set => UpdateProperty(ref _IntactStructNoInsertSummaryTable, value);
        }
    }
}
