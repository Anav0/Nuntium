using System;
using System.Globalization;
using System.Windows.Media;

namespace Nuntium
{
    public class StringToBrushConverter : BaseValueConverter<StringToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFromString((string)value));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
