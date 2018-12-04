using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{

    /// <summary>
    /// Converter given <see cref="byte" /> to readable format
    /// </summary>
    public class ByteSquasherConverter : BaseValueConverter<ByteSquasherConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (!(value is long size))
                return null;

            return size.ToReadableFormat(2);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
