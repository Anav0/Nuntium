using System;
using System.Globalization;
using System.Windows.Data;

namespace Nuntium
{
    public class EnumToBooleanConverter : BaseValueConverter<EnumToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
}
