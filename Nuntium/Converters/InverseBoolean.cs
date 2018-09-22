using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{
   
    public class InverseBoolean : BaseValueConverter<InverseBoolean>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is bool some))
                return null;

            return some ^= true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool some))
                return null;

            return some ^= true;
        }
    }
}
