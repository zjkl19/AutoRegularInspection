using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoRegularInspection.IRepository
{
    public interface IOptionPage
    {
        string Title { get; }
        UserControl Control { get; }
    }
}
