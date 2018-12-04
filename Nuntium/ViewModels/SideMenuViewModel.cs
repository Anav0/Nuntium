using Nuntium.Core;
using System.Windows.Input;

namespace Nuntium
{
    public class SideMenuViewModel : BaseViewModel
    {
        #region Public Properties

        public bool IsExpanded { get; set; }

        #endregion

        public SideMenuViewModel()
        {
            GoToInboxPageCommand = new RelayCommand(GoToInboxPage);
            ExpandCommand = new RelayCommand(Expand);
        }

        #region Public Commands

        public ICommand ExpandCommand { get; set; }

        public ICommand GoToInboxPageCommand { get; set; }

        #endregion

        #region Command Methods

        private void Expand()
        {
            IsExpanded ^= true;
        }

        private void GoToInboxPage()
        {
            //ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor);
        }

        #endregion
    }
}
