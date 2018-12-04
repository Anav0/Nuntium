using Nuntium;
using Nuntium.Core;
using System.Diagnostics;

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

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            if (page is TextEditor)
                return ApplicationPage.TextEditor;

            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
