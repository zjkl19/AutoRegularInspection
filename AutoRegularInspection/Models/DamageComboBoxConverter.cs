using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace AutoRegularInspection.Models
{
    public class DamageComboBoxConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<BridgeDamage> componentBox = GlobalData.ComponentComboBox;

            if ((BridgePart)parameter == BridgePart.BridgeDeck)
            {
                componentBox = GlobalData.ComponentComboBox;
            }
            else if ((BridgePart)parameter == BridgePart.SuperSpace)
            {
                componentBox = GlobalData.SuperSpaceComponentComboBox;
            }
            else
            {
                componentBox = GlobalData.SubSpaceComponentComboBox;
            }

            return componentBox[(int)value].DamageComboBox;
        }
        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }
    }
}
