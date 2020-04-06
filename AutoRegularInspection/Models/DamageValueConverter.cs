using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoRegularInspection.Models
{
    public class DamageValueConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GlobalData.ComponentComboBox[(int)value].DamageComboBox;
        }
        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
