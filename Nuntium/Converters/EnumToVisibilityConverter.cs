using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nuntium
{
    public class EnumToVisibilityGoneConverter : BaseValueConverter<EnumToVisibilityGoneConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(parameter))
                return Visibility.Visible;

            else return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
}
