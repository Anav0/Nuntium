
using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{
        public class StringToVisiblityInverseConverter : BaseValueConverter<StringToVisibilityConverter>
        {
            public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (string.IsNullOrEmpty((string)value))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

            public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
}
