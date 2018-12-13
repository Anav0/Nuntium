using Ninject;
using Nuntium.Core;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace Nuntium
{
    public class InboxPageViewModel : BaseViewModel
    {

        #region Private Members

        private List<MessageMiniatureViewModel> mRecentlyMovedMessages = new List<MessageMiniatureViewModel>();

        private SortingOption mContactsSortedBy;

        private InboxCategoryType mSelectedCategory;

        private SortingOption mMessagesSortedBy;

        private bool? mSelectionMode = false;

        #endregion

        #region Public properties

        public ContactsListViewModel ContactListVM { get; set; }

        public MessageMiniatureListViewModel MessageListVM { get; set; }

        public ObservableCollection<SortingOption> ContactsSortingOptions { get; set; }

        public ObservableCollection<SortingOption> MessagesSortingOptions { get; set; }

        public Dictionary<InboxCategoryType, ObservableCollection<MessageMiniatureViewModel>> Messages { get; set; } = new Dictionary<InboxCategoryType, ObservableCollection<MessageMiniatureViewModel>>();

        public SortingOption ContactsSortedBy
        {
            get => mContactsSortedBy;
            set
            {
                mContactsSortedBy = value;

                SortContacts();
            }
        }

        public InboxCategoryType SelectedCategory
        {
            get => mSelectedCategory;
            set
            {
                if (mSelectedCategory == value)
                    return;

                mSelectedCategory = value;

                GoToSelectedCategory();
                SortMessages();
                SelectMessages();
            }
        }

        public SortingOption MessagesSortedBy
        {
            get => mMessagesSortedBy;
            set
            {
                mMessagesSortedBy = value;

                SortMessages();
                SelectMessages();
            }
        }

        public bool IsRestoringArchivedMessages { get; set; }

        public bool IsRestorePopupShown { get; set; }

        public bool? SelectionMode
        {
            get => mSelectionMode;

            set
            {
                if (mSelectionMode != value)
                    mSelectionMode = value;

                SelectMessages();
            }
        }

        #endregion

        #region Public Commands

        public ICommand LoadTeachersCommand { get; set; }

        public ICommand LoadPupilsCommand { get; set; }

        public ICommand DeleteSelectedMessagesCommand { get; set; }

        public ICommand StarSelectedMessagesCommand { get; set; }

        public ICommand ArchiveSelectedMessagesCommand { get; set; }

        public ICommand RestoreDeletedMessagesCommand { get; set; }

        public ICommand ShowTextEditorCommand { get; set; }

        #endregion

        #region Constructor

        public InboxPageViewModel(IEmailService emailService, IEventAggregator eventAggregator)
        {
            InitializeCommands();
            InitializeSortingOptions();
            InitializeSubordinateViewModels();

            foreach (var category in (InboxCategoryType[])Enum.GetValues(typeof(InboxCategoryType)))
            {
                Messages.Add(category, new ObservableCollection<MessageMiniatureViewModel>());
            }

            GetEmailsFromService(emailService);

            LoadTeachers();
            GoToSelectedCategory();
            SortMessages();

            eventAggregator.GetEvent<EmailDeletedEvent>().Subscribe((emailId) => { DeleteEmail(emailId); });
            eventAggregator.GetEvent<EmailArchivedEvent>().Subscribe((emailId) => { ArchiveEmail(emailId); });
            eventAggregator.GetEvent<EmailStaredEvent>().Subscribe((emailId) => { StarEmail(emailId); });
        }

        #endregion

        #region Event handlers

        private void ArchiveEmail(string emailId)
        {
            var email = MessageListVM.Items.First(x => x.Id == emailId);

            IsRestoringArchivedMessages = true;

            mRecentlyMovedMessages.Clear();
            mRecentlyMovedMessages.Add(email);

            Messages[InboxCategoryType.Archive].Add(email);
            Messages[email.Placement].Remove(email);

            email.PrevPlacement = email.Placement;
            email.Placement = InboxCategoryType.Archive;

            MessageListVM.Items.Remove(email);

            DisplayRestorationPopup();

        }

        public void DeleteEmail(string emailId)
        {
            var email = MessageListVM.Items.First(x => x.Id == emailId);

            IsRestoringArchivedMessages = false;

            mRecentlyMovedMessages.Clear();

            //Add message to recently deleted ones
            mRecentlyMovedMessages.Add(email);

            MoveToDeletedList(email);

            //Update placement of the message
            email.PrevPlacement = email.Placement;
            email.Placement = InboxCategoryType.Deleted;

            //Delete message from displayed list
            MessageListVM.Items.Remove(email);

            DisplayRestorationPopup();
        }

        private void StarEmail(string emailId)
        {
            var email = MessageListVM.Items.First(x => x.Id == emailId);

            if (email.IsStared)
            {
                Messages[InboxCategoryType.Stared].Add(email);
            }
            else
            {
                Messages[InboxCategoryType.Stared].Remove(email);
            }

        }

        #endregion

        #region Command Methods

        private void LoadTeachers()
        {
            ContactListVM.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListVM.AllItems.FindAll(x => x.IsStudent == false));
        }

        private void LoadPupils()
        {
            ContactListVM.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListVM.AllItems.FindAll(x => x.IsStudent));
        }

        private void SelectMessages()
        {
            switch (SelectionMode)
            {
                case null:
                    MessageListVM.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items);
                    break;

                case true:
                    switch (MessagesSortedBy.SortedBy)
                    {
                        case SortedBy.Unread:
                            MessageListVM.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.Where(x => !x.WasRead));
                            break;

                        case SortedBy.Read:
                            MessageListVM.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.Where(x => x.WasRead));
                            break;

                        case SortedBy.Stared:
                            MessageListVM.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.Where(x => x.IsStared));
                            break;

                        case SortedBy.Unstared:
                            MessageListVM.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.Where(x => !x.IsStared));
                            break;

                        default:
                            MessageListVM.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items);
                            break;
                    }
                    break;

                case false:
                    MessageListVM.SelectedItems.Clear();
                    break;
            }
        }

        private void RestoreMessages()
        {
            IsRestorePopupShown = false;

            mRecentlyMovedMessages.ForEach(item =>
            {
                if (item.IsStared && !Messages[InboxCategoryType.Stared].Contains(item))
                    Messages[InboxCategoryType.Stared].Add(item);

                if (item.PrevPlacement == InboxCategoryType.Archive)
                    item.IsArchived = true;
                else item.IsArchived = false;

                Messages[item.Placement].Remove(item);
                Messages[item.PrevPlacement].Add(item);

                var tmp = item.Placement;
                item.Placement = item.PrevPlacement;
                item.PrevPlacement = tmp;
            });

            GoToSelectedCategory();
            SortMessages();
            SelectMessages();
        }

        private void DeleteSelectedMessages()
        {
            new Thread(() =>
            {
                IsRestoringArchivedMessages = false;

                mRecentlyMovedMessages = new List<MessageMiniatureViewModel>(MessageListVM.SelectedItems);

                for (int i = MessageListVM.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var item = MessageListVM.SelectedItems[i];

                    MoveToDeletedList(item);

                    item.PrevPlacement = item.Placement;
                    item.Placement = InboxCategoryType.Deleted;
                }

                GoToSelectedCategory();
                SortMessages();
                DisplayRestorationPopup();

            }).Start();

        }

        private void ArchiveSelectedMessages()
        {
            //don't archive archived messages
            if (SelectedCategory == InboxCategoryType.Archive)
                return;

            new Thread(() =>
            {
                IsRestoringArchivedMessages = true;

                mRecentlyMovedMessages = new List<MessageMiniatureViewModel>(MessageListVM.SelectedItems);

                for (int i = MessageListVM.SelectedItems.Count - 1; i >= 0; i--)
                {
                    MessageListVM.SelectedItems[i].IsArchived = true;

                    Messages[InboxCategoryType.Archive].Add(MessageListVM.SelectedItems[i]);
                    Messages[MessageListVM.SelectedItems[i].Placement].Remove(MessageListVM.SelectedItems[i]);

                    MessageListVM.SelectedItems[i].PrevPlacement = MessageListVM.SelectedItems[i].Placement;
                    MessageListVM.SelectedItems[i].Placement = InboxCategoryType.Archive;

                }

                GoToSelectedCategory();
                SortMessages();
                DisplayRestorationPopup();

            }).Start();

        }

        private void ToggleStarForSelectedMesseges()
        {
            foreach (var message in MessageListVM.SelectedItems)
            {
                message.IsStared = true;

                if (!Messages[InboxCategoryType.Stared].Contains(message))
                    Messages[InboxCategoryType.Stared].Add(message);
            }
        }

        private void ShowTextEditor()
        {
            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor, new TextEditorViewModel());
        }

        #endregion

        #region Private Methods

        private void GetEmailsFromService(IEmailService emailService)
        {
            foreach (var email in emailService.GetAllEmails())
            {
                var msg = new MessageMiniatureViewModel(email.Id)
                {
                    AvatarBackground = ColorHelpers.GenerateRandomColor(),
                    HasAttachments = email.Attachments.Count > 0 ? true : false,
                    SendDate = email.SendDate,
                    SenderName = email.SenderName,
                    Subject = email.Subject,
                    Placement = InboxCategoryType.Inbox,
                    Message = email.Message

                };

                Messages[InboxCategoryType.Inbox].Add(msg);
            }
        }

        private void InitializeSubordinateViewModels()
        {
            ContactListVM = new ContactsListViewModel
            {
                AllItems = new List<ContactViewModel>
                {

                    new ContactViewModel
                    {
                        PersonName = "Tomasz Zabłocki",
                        Initials = "TJ",
                        PersonPosition = "English teacher"

                    },
                    new ContactViewModel
                    {
                        PersonName = "Krzysztof Jarzyna",
                        Initials = "KJ",
                        PersonPosition = "Head teacher"
                    },
                     new ContactViewModel
                    {
                        PersonName = "Andrzej Górski",
                        Initials = "AG",
                        PersonPosition = "Head teacher"
                    },
                      new ContactViewModel
                    {
                        PersonName = "Jacek Tabisz",
                        Initials = "JT",
                        PersonPosition = "History teacher",
                        HasNewMessages = true
                    },
                    new ContactViewModel
                    {
                        PersonName = "Paweł Wrona",
                        Initials = "PW",
                        PersonPosition = "Anatomy teacher",
                        HasNewMessages = true
                    },
                      new ContactViewModel
                    {
                        PersonName = "Piotr Smutny",
                        Initials = "PS",
                        IsStudent = true,
                        PersonPosition = "2b",
                    },
                        new ContactViewModel
                    {
                        PersonName = "Jacek Trąbka",
                        Initials = "JT",
                        IsStudent = true,
                        PersonPosition = "3c",
                    },
                    new ContactViewModel
                    {
                        PersonName = "Weronika Zakrzewska",
                        Initials = "WZ",
                        IsStudent = true,
                        PersonPosition = "5a",
                    },
                    new ContactViewModel
                    {
                        PersonName = "Kasia Strzelczyk",
                        Initials = "KS",
                        IsStudent = true,
                        PersonPosition = "6c"

                    },
                     new ContactViewModel
                    {
                        PersonName = "Weronika Zakrzewska",
                        Initials = "WZ",
                        IsStudent = true,
                        PersonPosition = "5b",
                    },
                    new ContactViewModel
                    {
                        PersonName = "Ludwik",
                        Initials = "LL",
                        IsStudent = true,
                        PersonPosition = "5a"
                    },
                },
            };

            MessageListVM = new MessageMiniatureListViewModel();

            MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>();

            ContactListVM.AllItems.Sort((y, x) => string.Compare(y.PersonName, x.PersonName));

        }

        private void InitializeSortingOptions()
        {
            ContactsSortingOptions = new ObservableCollection<SortingOption>
            {
                new SortingOption
                {
                    Id = 0,
                    Option = "Alphabetically",
                    SortedBy = Core.SortedBy.Alphabetical
                },
                new SortingOption
                {
                    Id = 1,
                    Option = "Position",
                    SortedBy = Core.SortedBy.Position
                },

                new SortingOption
                {
                    Id = 2,
                    Option = "New messages",
                    SortedBy = Core.SortedBy.NewMessage
                },
            };

            MessagesSortingOptions = new ObservableCollection<SortingOption>
            {
                new SortingOption
                {
                    Id = 0,
                    Option = "By date",
                    SortedBy = Core.SortedBy.Date
                },
                new SortingOption
                {
                    Id = 1,
                    Option = "Alphabetically",
                    SortedBy = Core.SortedBy.Alphabetical
                },
                new SortingOption
                {
                    Id = 2,
                    Option = "By date",
                    SortedBy = Core.SortedBy.Date
                },

                new SortingOption
                {
                    Id = 3,
                    Option = "Unread",
                    SortedBy = Core.SortedBy.Unread
                },

                 new SortingOption
                {
                    Id = 4,
                    Option = "Read",
                    SortedBy = Core.SortedBy.Read
                },

            };
        }

        private void InitializeCommands()
        {
            LoadPupilsCommand = new RelayCommand(LoadPupils);
            LoadTeachersCommand = new RelayCommand(LoadTeachers);
            RestoreDeletedMessagesCommand = new RelayCommand(RestoreMessages);

            DeleteSelectedMessagesCommand = new RelayCommand(DeleteSelectedMessages);
            StarSelectedMessagesCommand = new RelayCommand(ToggleStarForSelectedMesseges);
            ArchiveSelectedMessagesCommand = new RelayCommand(ArchiveSelectedMessages);
            ShowTextEditorCommand = new RelayCommand(ShowTextEditor);
        }

        private void SortContacts()
        {
            if (ContactsSortedBy == null)
                return;

            switch (ContactsSortedBy.SortedBy)
            {
                case SortedBy.Alphabetical:
                    ContactListVM.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListVM.FilteredItems.OrderBy(x => x.PersonName));
                    break;
                case SortedBy.Class:
                case SortedBy.Position:
                    ContactListVM.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListVM.FilteredItems.OrderBy(x => x.PersonPosition));
                    break;
                case SortedBy.NewMessage:
                    ContactListVM.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListVM.FilteredItems.OrderBy(x => x.HasNewMessages ? 0 : 1));
                    break;

            }
        }

        private void SortMessages()
        {
            if (MessagesSortedBy == null)
                return;

            switch (MessagesSortedBy.SortedBy)
            {
                case Core.SortedBy.Alphabetical:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderBy(x => x.Subject));
                    break;
                case Core.SortedBy.Date:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderByDescending(x => x.SendDate));
                    break;
                case Core.SortedBy.Unread:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderBy(x => x.WasRead ? 1 : 0));
                    break;
                case Core.SortedBy.Read:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderBy(x => x.WasRead ? 0 : 1));
                    break;
                case Core.SortedBy.Stared:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderBy(x => x.IsStared ? 1 : 0));
                    break;
                case Core.SortedBy.Unstared:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderBy(x => x.IsStared ? 0 : 1));
                    break;
                case Core.SortedBy.Author:
                    MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListVM.Items.OrderBy(x => x.SenderName));
                    break;
            }
        }

        private void GoToSelectedCategory()
        {
            MessageListVM.Items = new ObservableCollection<MessageMiniatureViewModel>(Messages[SelectedCategory]);
        }

        private void DisplayRestorationPopup()
        {
            IsRestorePopupShown = true;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Elapsed += (s, e) =>
            {
                IsRestorePopupShown = false;
                timer.Stop();
            };
            timer.Start();
        }

        private void MoveToDeletedList(MessageMiniatureViewModel message)
        {
            message.IsArchived = false;

            //Remove message from category list its on.
            Messages[message.Placement].Remove(message);

            //If message is being permamently deleted delete it's star also
            if (message.Placement == InboxCategoryType.Deleted)
            {
                Messages[InboxCategoryType.Stared].Remove(message);
            }

            //If message is already deleted don't add it to the bin.
            if (message.Placement != InboxCategoryType.Deleted)
                Messages[InboxCategoryType.Deleted].Add(message);
        }

        #endregion

    }
}
