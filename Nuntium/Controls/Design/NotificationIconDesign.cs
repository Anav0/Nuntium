using Nuntium;
using Nuntium.Core;

namespace Nuntium
{
    public class NotificationIconDesign : NotificationIconViewModel
    {
        public static NotificationIconDesign Instance => new NotificationIconDesign();

        public NotificationIconDesign()
        {
            NumberOfNotifications = 10;
            Icon = IconType.Bell;
            ButtonForeground = "#ffffff";
        }
    }
}
