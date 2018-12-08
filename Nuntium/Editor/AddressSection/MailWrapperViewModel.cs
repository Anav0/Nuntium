
using Nuntium.Core;
using System;
using System.Windows.Input;

namespace Nuntium
{
    public class MailWrapperViewModel : BaseViewModel
    {

        #region Public properties

        public double CornerRadius { get; set; } = 10;

        public string Address { get; set; }

        public string FirstLetter { get; set; }

        public string BubbleColor { get; set; } = "#FF7F50";

        #endregion

        public ICommand DeleteCommand { get; set; }

        public MailWrapperViewModel()
        {

        }

    }
}
