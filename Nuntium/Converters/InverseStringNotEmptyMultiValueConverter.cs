using System;
using System.Globalization;
using System.Windows.Data;

namespace Nuntium
{
    public class InverseStringNotEmptyMultiValueConverter : BaseMultiValueConverter<InverseStringNotEmptyMultiValueConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return null;

            foreach(var value in values)
            {
                if ((value is string) && string.IsNullOrWhiteSpace((string)value))
                    return false;
            }

            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
