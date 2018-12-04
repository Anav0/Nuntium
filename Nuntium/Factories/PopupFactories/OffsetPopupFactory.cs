
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Nuntium
{
    public class OffsetPopupFactory : IPopupFactory
    {
        public Popup CreatePopupOnPoint(Point point)
        {
            var popup = new Popup();
            popup.StaysOpen = false;
            popup.HorizontalOffset = point.X;
            popup.VerticalOffset = point.Y;

            return popup;
        }
    }
}
