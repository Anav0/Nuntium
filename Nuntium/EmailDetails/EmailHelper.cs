using Nuntium.Core;
using Prism.Events;
using System.Collections.Generic;

namespace Nuntium
{
    public class EmailHelper
    {
        public void ShowEmailDetails(
            MessageMiniatureViewModel email, 
            IEmailService emailService,
            IEventAggregator eventAggregator,
            ICatalogService catalogService, 
            CustomRichTextBox editor, 
            AddressSectionViewModel addressSectionViewModel
            )
        {
            //Mark as read
            //TODO: This function is responsible for two things(marking as read and displaying email details.
            //Change so it will not break SOLID design principal.
            email.WasRead = true;

            var emailDetailsVM = new EmailDetailsPageViewModel(email.Id, emailService, editor, addressSectionViewModel, email.AvatarBackground, eventAggregator, catalogService);

            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.EmailDetailsPage, emailDetailsVM);
        }

       
    }
}
