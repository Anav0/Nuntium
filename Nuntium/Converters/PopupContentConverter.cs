using Nuntium.Core;
using System;
using System.Globalization;
using System.Windows;

namespace Nuntium
{
    public class PopupContentConverter : BaseValueConverter<PopupContentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is UserPopupViewModel baseViewModel)
            //    return new UserPopup { DataContext = baseViewModel };


            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
