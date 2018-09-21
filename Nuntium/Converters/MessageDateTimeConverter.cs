using System;
using System.Globalization;

namespace Nuntium
{
    public class MessageDateTimeConverter : BaseValueConverter<MessageDateTimeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is DateTime date))
                return null;

            var diff = (DateTime.Now - date).TotalDays;

            if(diff < 1)
                    return date.ToString("HH:mm");

            return date.ToString("dd MMM");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
