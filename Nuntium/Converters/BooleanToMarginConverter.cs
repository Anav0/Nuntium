using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{
    public class BooleanToMarginConverter : BaseValueConverter<BooleanToMarginConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool)value)
            {
                return new Thickness(65, 15, 15, 15);
            }
            else
            {
                return new Thickness(15);
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
