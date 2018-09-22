using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{
    public class NumberToBooleanConverter : BaseValueConverter<NumberToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
