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

                case ApplicationPage.Inbox:
                    return new InboxPage(viewModel as InboxPageViewModel);

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            if (page is InboxPage)
                return ApplicationPage.Inbox;

            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
