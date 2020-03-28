using AutoRegularInspection.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoRegularInspection.Models
{

    //[ValueConversion(typeof(int), typeof(string))]
    public class EnumConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enmus = value as string[];
            List<string> result = new List<string>();
            foreach (var item in enmus)
            {
                switch (item)
                {
                    case "DeckPavement":
                        result.Add("桥面铺装");
                        break;
                    case "SelectedTwo":
                        result.Add("第二行");
                        break;
                    case "SelectedThird":
                        result.Add("第三行");
                        break;
                }
            }
            return result;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
