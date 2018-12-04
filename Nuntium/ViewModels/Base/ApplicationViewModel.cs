
using Nuntium.Core;

namespace Nuntium
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public Properties

        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.TextEditor;

        public BaseViewModel CurrentPageViewModel { get; set; }

        #endregion


        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            if (page == CurrentPage)
                return;

            CurrentPageViewModel = viewModel;

            var different = CurrentPage != page;

            CurrentPage = page;

            if (!different)
                OnPropertyChanged(nameof(CurrentPage));
        }

    }
}
