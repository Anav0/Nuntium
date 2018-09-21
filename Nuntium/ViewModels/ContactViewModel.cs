using Nuntium.Core;
using System.Windows.Media;

namespace Nuntium
{
    public class ContactViewModel : BaseViewModel
    {
        #region Public Properties

        public string PersonName { get; set; }

        public string Initials { get; set; }

        public string PersonPosition { get; set; }

        public string AvatarBackground { get; set; } = ColorHelpers.GenerateRandomColor();

        public bool HasNewMessages { get; set; }

        public bool IsStudent { get; set; }


        #endregion
    }
}
