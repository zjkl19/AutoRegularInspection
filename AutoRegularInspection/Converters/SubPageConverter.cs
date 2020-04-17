using AutoRegularInspection.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutoRegularInspection.Converters
{
    public class SubPageConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(System.Convert.ToInt32(value)==1)
            {
                return new Frame
                {
                    Content = new Page1()
                ,
                    Tag = "Page1"
                };
            }
            else if(System.Convert.ToInt32(value) == 2)
            {
                return new Frame
                {
                    Content = new Page2()
                ,
                    Tag = "Page1"
                };
            }
            else
            {
                return null;
            }
        }
        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (int)value;
            int result;
            if ((int)parameter == 1)
            {
                result = 1;
            }
            else if ((int)parameter == 2)
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
