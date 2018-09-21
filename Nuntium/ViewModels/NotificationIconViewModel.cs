using Nuntium.Core;
using System.Windows.Input;

namespace Nuntium
{
    public class NotificationIconViewModel : BaseViewModel
    {
        #region Public Properties

        public IconType Icon { get; set; } = IconType.Bell;

        public int NumberOfNotifications { get; set; } = 0;

        public bool IsPressed { get; set; } = false;

        public string ButtonForeground { get; set; } = "#ffffff";

        #endregion

        #region Public Commands

        public ICommand ClickCommand { get; set; }

        #endregion

        public NotificationIconViewModel()
        {
            ClickCommand = new RelayCommand(Click);
        }

        private void Click()
        {
            IsPressed ^= true;

            NumberOfNotifications++;
        }
    }
}
