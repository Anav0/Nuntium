
using System;
using System.Globalization;
using Nuntium.Core;

namespace Nuntium
{
    public class NameToInitialsConverter : BaseValueConverter<NameToInitialsConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!(value is string name))
                return null;

            return name.GetInitials();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
