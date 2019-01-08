using Ninject;
using Nuntium.Core;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Nuntium
{
    public class MessageMiniatureViewModel : BaseViewModel
    {
        private IEventAggregator mEventAggregator;

        #region Public properties

        public string Id { get; set; }

        public string AvatarBackground { get; set; }

        public string ImagePath { get; set; }

        public string SenderName { get; set; }

        public string Subject { get; set; }

        public DateTime SendDate { get; set; }

        public bool HasAttachments { get; set; }

        public bool WasRead { get; set; }

        public bool IsStared { get; set; }

        public bool IsArchived { get; set; }

        public bool AnimateOut { get; set; }

        public InboxCategoryType Placement { get; set; }

        public InboxCategoryType PrevPlacement { get; set; }

        public string Message { get; set; }

        public TimeSpan AnimateOutTimeSpan { get; set; } = new TimeSpan(0, 0, 0, 0, 500);

        #endregion

        public MessageMiniatureViewModel(string emailId, IEventAggregator eventAggregator)
        {
            mEventAggregator = eventAggregator;
            Id = emailId;
            DeleteCommand = new RelayCommandWithParameter((parameter) => Delete(parameter));
            ToggleStarCommand = new RelayCommand(ToggleStar);
            ToggleArchiveCommand = new RelayCommandWithParameter((parameter) => Archive(parameter));
            ShowEmailDetailsCommand = new RelayCommand(ShowEmailDetails);
        }

        #region Public Commands

        public ICommand DeleteCommand { get; set; }

        public ICommand ToggleStarCommand { get; set; }

        public ICommand ToggleArchiveCommand { get; set; }

        public ICommand ShowEmailDetailsCommand { get; set; }

        #endregion

        #region Command Methods

        private void ToggleStar()
        {
            IsStared ^= true;
            mEventAggregator.GetEvent<EmailStaredEvent>().Publish(Id);
        }

        private async void Archive(object param)
        {
            if (!(param is FrameworkElement element))
                return;

            if (IsArchived)
                return;

            await FrameworkElementAnimation.AnimateOut(element, AnimationDirection.Right, new Duration(AnimateOutTimeSpan), true, 0.25);

            IsArchived = true;

            mEventAggregator.GetEvent<EmailArchivedEvent>().Publish(Id);
        }

        private async void Delete(object param)
        {
            if (!(param is FrameworkElement element))
                return;

            await FrameworkElementAnimation.AnimateOut(element, AnimationDirection.Left, new Duration(AnimateOutTimeSpan), true, 0.25);

            mEventAggregator.GetEvent<EmailDeletedEvent>().Publish(Id);

        }

        private void ShowEmailDetails()
        {
            //Mark as read
            //TODO: This function is responsible for two things(marking as read and displaying email details.
            //Change so it will not break SOLID design principal.
            WasRead = true;

            var emailService = IoC.Kernel.Get<IEmailService>();
            var editor = IoC.Kernel.Get<CustomRichTextBox>();
            var adrSectionVM = IoC.Kernel.Get<AddressSectionViewModel>();
            var catalogService = IoC.Kernel.Get<ICatalogService>();

            var emailDetailsVM = new EmailDetailsPageViewModel(Id, emailService, editor, adrSectionVM, AvatarBackground, mEventAggregator, catalogService);

            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.EmailDetailsPage, emailDetailsVM);
        }

    }


    #endregion
}

