
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Nuntium
{
    public interface IPopupFactory
    {
        Popup CreatePopupOnPoint(Point point);
    }
}
