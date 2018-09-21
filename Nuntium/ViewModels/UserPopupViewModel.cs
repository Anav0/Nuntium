using System;
using System.Windows.Input;

namespace Nuntium
{
    public class UserPopupViewModel : BasePopupViewModel
    {
        #region Public Properties

        public string Fullname { get; set; }

        public string Position { get; set; }

        public string UserInitials { get; set; }

        public string UserBackground { get; set; }

        public string ImagePath { get; set; }

        #endregion

        #region Public Command

        public ICommand SendMessageCommand { get; set; }

        #endregion

        public UserPopupViewModel()
        {
            SendMessageCommand = new RelayCommand(SendMessage);
        }
        #region Public Command Methods

        private void SendMessage()
        {

        }

        #endregion
    }
}
