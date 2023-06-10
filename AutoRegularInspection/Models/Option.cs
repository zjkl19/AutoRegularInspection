using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoRegularInspection.Models
{
    public class Option
    {
        public string Name { get; set; }
        public UserControl UserControl { get; set; }
        public List<Option> Children { get; set; }
    }
}
