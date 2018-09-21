using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{
    /// <summary>
    /// A converter that takes in a boolean and inverts it
    /// </summary>
    public class InverseBooleanToVisibilityGoneConverter : BaseValueConverter<InverseBooleanToVisibilityGoneConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
