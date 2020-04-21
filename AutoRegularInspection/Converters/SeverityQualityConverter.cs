using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoRegularInspection.Converters
{
    public class SeverityQualityConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result;
            if (System.Convert.ToInt32(parameter) == 1)
            {
                result = 1;
            }
            else if (System.Convert.ToInt32(parameter) == 2)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }

            return result;
        }
        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (int)value;
            int result;
            if (System.Convert.ToInt32(parameter) == 1)
            {
                result = 1;
            }
            else if (System.Convert.ToInt32(parameter) == 2)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }

            return result;
        }
    }
}
