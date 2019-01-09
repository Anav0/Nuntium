
using Nuntium.Core;
using System.Windows;
using System.Windows.Input;

namespace Nuntium
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public Properties

        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Blank;

        public BaseViewModel CurrentPageViewModel { get; set; }

        #endregion

        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            if (page == CurrentPage && page != ApplicationPage.EmailDetailsPage)
                return;

            CurrentPageViewModel = viewModel;

            var different = CurrentPage != page;

            CurrentPage = page;

            if (!different)
                OnPropertyChanged(nameof(CurrentPage));
        }

        public void ShowModal(string message, string header, string yesButtonText, string noButtonText, ICommand yesAction, ICommand noCommand)
        {
            var modal = new DefaultModal
            {
                Message = message,
                Header = header,
                YesButtonText = yesButtonText,
                NoButtonText = noButtonText,
            };

            Window window = new Window
            {
                Content = modal,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            modal.YesCommand = new RelayCommand(() =>
            {
                yesAction?.Execute(null);
                window.Close();
            });

            modal.NoCommand = new RelayCommand(() => 
            {
                noCommand?.Execute(null);
                window.Close();
            });

            window.ShowDialog();
        }
    }
}
