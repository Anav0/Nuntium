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

        public MessageMiniatureViewModel(
            string emailId, 
            IEventAggregator eventAggregator, 
            IEmailService emailService,
            ICatalogService catalogService, 
            CustomRichTextBox editor, 
            AddressSectionViewModel addressSectionViewModel
            )
        {
            mEventAggregator = eventAggregator;
            Id = emailId;
            DeleteCommand = new RelayCommandWithParameter((parameter) => Delete(parameter));
            ToggleStarCommand = new RelayCommand(Star);
            ToggleArchiveCommand = new RelayCommandWithParameter((parameter) => Archive(parameter));
            ShowEmailDetailsCommand = new RelayCommand(() => 
            {
                new EmailHelper().ShowEmailDetails
                (
                   this,
                   emailService,
                   eventAggregator,
                   catalogService,
                   editor,
                   addressSectionViewModel
                );
            });
        }

        #region Public Commands

        public ICommand DeleteCommand { get; set; }

        public ICommand ToggleStarCommand { get; set; }

        public ICommand ToggleArchiveCommand { get; set; }

        public ICommand ShowEmailDetailsCommand { get; set; }

        #endregion

        #region Command Methods

        private void Star()
        {
            mEventAggregator.GetEvent<EmailStaredEvent>().Publish(Id);
        }

        private async void Archive(object callingElement)
        {
            if (!(callingElement is FrameworkElement element))
                return;

            if (IsArchived)
                return;

            await FrameworkElementAnimation.AnimateOut(element, AnimationDirection.Right, new Duration(AnimateOutTimeSpan), true, 0.25);

            mEventAggregator.GetEvent<EmailArchivedEvent>().Publish(Id);
        }

        private async void Delete(object callingElement)
        {
            if (!(callingElement is FrameworkElement element))
                return;

            await FrameworkElementAnimation.AnimateOut(element, AnimationDirection.Left, new Duration(AnimateOutTimeSpan), true, 0.25);

            mEventAggregator.GetEvent<EmailDeletedEvent>().Publish(Id);

        }

    }


    #endregion
}

