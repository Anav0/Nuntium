using Nuntium;
using Nuntium.Core;
using System.Diagnostics;
using System.Windows.Controls;

namespace Nuntium
{
    public static class ApplicationPageExtentions
    {
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            switch (page)
            {

                case ApplicationPage.TextEditor:
                    return new TextEditor(viewModel as TextEditorViewModel);

                case ApplicationPage.EmailDetailsPage:
                    return new EmailDetailsPage(viewModel as EmailDetailsPageViewModel);

                case ApplicationPage.Blank:
                    return new BlankPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            if (page is TextEditor)
                return ApplicationPage.TextEditor;

            if (page is EmailDetailsPage)
                return ApplicationPage.EmailDetailsPage;

            else return default(ApplicationPage);
        }
    }
}
