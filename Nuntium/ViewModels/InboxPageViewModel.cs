using Nuntium.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Nuntium
{
    public class InboxPageViewModel : BaseViewModel
    {

        #region Private Members

        private List<MessageMiniatureViewModel> mRecentlyMovedMessages = new List<MessageMiniatureViewModel>();

        private SortingOption _ContactsSortedBy;

        private InboxCategoryType _SelectedCategory;

        private SortingOption _MessagesSortedBy;

        private bool? _SelectionMode = false;

        #endregion

        #region Public properties

        public ContactsListViewModel ContactListData { get; set; }

        public MessageMiniatureListViewModel MessageListData { get; set; }

        public ObservableCollection<SortingOption> ContactsSortingOptions { get; set; }

        public ObservableCollection<SortingOption> MessagesSortingOptions { get; set; }

        public Dictionary<InboxCategoryType, ObservableCollection<MessageMiniatureViewModel>> Messages { get; set; } = new Dictionary<InboxCategoryType, ObservableCollection<MessageMiniatureViewModel>>();

        public SortingOption ContactsSortedBy
        {
            get => _ContactsSortedBy;
            set
            {
                _ContactsSortedBy = value;

                SortContacts();
            }
        }

        public InboxCategoryType SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                if (_SelectedCategory == value)
                    return;

                _SelectedCategory = value;

                GoToCategory();
                SortMessages();
                SelectMessages();
            }
        }

        public SortingOption MessagesSortedBy
        {
            get => _MessagesSortedBy;
            set
            {
                _MessagesSortedBy = value;

                SortMessages();
                SelectMessages();
            }
        }

        public bool IsRestoringArchivedMessages { get; set; }

        public bool IsRestorePopupShown { get; set; }

        public bool? SelectionMode
        {
            get => _SelectionMode;

            set
            {
                if (_SelectionMode != value)
                    _SelectionMode = value;

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

        public InboxPageViewModel()
        {
            var rnd = new Random();

            LoadPupilsCommand = new RelayCommand(LoadPupils);
            LoadTeachersCommand = new RelayCommand(LoadTeachers);
            RestoreDeletedMessagesCommand = new RelayCommand(RestoreMessages);

            DeleteSelectedMessagesCommand = new RelayCommand(DeleteSelectedMessages);
            StarSelectedMessagesCommand = new RelayCommand(ToggleStarForSelectedMesseges);
            ArchiveSelectedMessagesCommand = new RelayCommand(ArchiveSelectedMessages);
            ShowTextEditorCommand = new RelayCommand(ShowTextEditor);
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

            ContactListData = new ContactsListViewModel
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

            MessageListData = new MessageMiniatureListViewModel();

            MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>();

            foreach (var category in (InboxCategoryType[])Enum.GetValues(typeof(InboxCategoryType)))
            {
                Messages.Add(category, new ObservableCollection<MessageMiniatureViewModel>());
            }

            //TODO: delete after frontend is done
            for (int i = 0; i < 100; i++)
            {
                var msg = new MessageMiniatureViewModel
                {
                    Id = i.ToString(),
                    AvatarBackground = ColorHelpers.GenerateRandomColor(),
                    HasAttachments = rnd.Next(0, 100) < 80,
                    SendDate = DateHelper.RandomDate(),
                    SenderName = Faker.Name.FullName(Faker.NameFormats.Standard),
                    Title = Faker.Lorem.Sentence(),
                    Placement = InboxCategoryType.Inbox,
                    WasRead = rnd.Next(0, 100) < 60,
                    MessageSnipit = string.Join(" ", Faker.Lorem.Sentences(10))

                };
                msg.OnItemDeleted += DeleteMessage;
                msg.OnItemArchived += ArchiveMessage;
                msg.OnItemStared += ToggleStarState;
                msg.Initials = msg.SenderName.GetInitials();

                Messages[InboxCategoryType.Inbox].Add(msg);
            }

            ContactListData.AllItems.Sort((y, x) => string.Compare(y.PersonName, x.PersonName));

            LoadTeachers();
            GoToCategory();
            SortMessages();
        }

       

        #endregion

        #region Event handlers

        private void ArchiveMessage(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            IsRestoringArchivedMessages = true;

            mRecentlyMovedMessages.Clear();
            mRecentlyMovedMessages.Add(item);

            Messages[InboxCategoryType.Archive].Add(item);
            Messages[item.Placement].Remove(item);

            item.PrevPlacement = item.Placement;
            item.Placement = InboxCategoryType.Archive;

            //Delete message from displayed list
            MessageListData.Items.Remove(item);

            DisplayRestorationPopup();

        }

        private void ToggleStarState(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            if (item.IsStared)
            {
                Messages[InboxCategoryType.Stared].Add(item);
            }
            else
            {
                Messages[InboxCategoryType.Stared].Remove(item);
            }
        }

        private void DeleteMessage(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            IsRestoringArchivedMessages = false;

            mRecentlyMovedMessages.Clear();

            //Add message to recently deleted ones
            mRecentlyMovedMessages.Add(item);

            MoveToDeletedList(item);

            //Update placement of the message
            item.PrevPlacement = item.Placement;
            item.Placement = InboxCategoryType.Deleted;

            //Delete message from displayed list
            MessageListData.Items.Remove(item);

            DisplayRestorationPopup();
        }

        private void OnItemStared(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            if (item.IsStared)
            {
                Messages[InboxCategoryType.Stared].Add(item);
            }
            else
            {
                Messages[InboxCategoryType.Stared].Remove(item);
            }

        }

        private void OnItemArchived(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            if (item.IsArchived)
            {
                Messages[item.Placement].Remove(item);
                Messages[InboxCategoryType.Archive].Add(item);

                item.PrevPlacement = item.Placement;
                item.Placement = InboxCategoryType.Archive;

            }
            else
            {
                Messages[item.Placement].Remove(item);
                Messages[item.PrevPlacement].Add(item);

                var tmp = item.Placement;
                item.Placement = item.PrevPlacement;
                item.PrevPlacement = tmp;
            }

            GoToCategory();
            SortMessages();
        }

        #endregion

        #region Command Methods

        private void LoadTeachers()
        {
            ContactListData.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListData.AllItems.FindAll(x => x.IsStudent == false));

            SortContacts();
        }

        private void LoadPupils()
        {
            ContactListData.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListData.AllItems.FindAll(x => x.IsStudent));

            SortContacts();
        }

        private void SelectMessages()
        {
            switch (SelectionMode)
            {
                case null:
                    MessageListData.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items);
                    break;

                case true:
                    switch (MessagesSortedBy.SortedBy)
                    {
                        case SortedBy.Unread:
                            MessageListData.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.Where(x => !x.WasRead));
                            break;

                        case SortedBy.Read:
                            MessageListData.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.Where(x => x.WasRead));
                            break;

                        case SortedBy.Stared:
                            MessageListData.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.Where(x => x.IsStared));
                            break;

                        case SortedBy.Unstared:
                            MessageListData.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.Where(x => !x.IsStared));
                            break;

                        default:
                            MessageListData.SelectedItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items);
                            break;
                    }
                    break;

                case false:
                    MessageListData.SelectedItems.Clear();
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

            GoToCategory();
            SortMessages();
            SelectMessages();
        }

        private void DeleteSelectedMessages()
        {
            new Thread(() =>
            {
                IsRestoringArchivedMessages = false;

                mRecentlyMovedMessages = new List<MessageMiniatureViewModel>(MessageListData.SelectedItems);

                for (int i = MessageListData.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var item = MessageListData.SelectedItems[i];

                    MoveToDeletedList(item);

                    item.PrevPlacement = item.Placement;
                    item.Placement = InboxCategoryType.Deleted;
                }

                GoToCategory();
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

                mRecentlyMovedMessages = new List<MessageMiniatureViewModel>(MessageListData.SelectedItems);

                for (int i = MessageListData.SelectedItems.Count - 1; i >= 0; i--)
                {
                    MessageListData.SelectedItems[i].IsArchived = true;

                    Messages[InboxCategoryType.Archive].Add(MessageListData.SelectedItems[i]);
                    Messages[MessageListData.SelectedItems[i].Placement].Remove(MessageListData.SelectedItems[i]);

                    MessageListData.SelectedItems[i].PrevPlacement = MessageListData.SelectedItems[i].Placement;
                    MessageListData.SelectedItems[i].Placement = InboxCategoryType.Archive;

                }

                GoToCategory();
                SortMessages();
                DisplayRestorationPopup();

            }).Start();

        }

        private void ToggleStarForSelectedMesseges()
        {
            foreach (var message in MessageListData.SelectedItems)
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

        private void SortContacts()
        {
            if (ContactsSortedBy == null)
                return;

            switch (ContactsSortedBy.SortedBy)
            {
                case Core.SortedBy.Alphabetical:
                    ContactListData.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListData.FilteredItems.OrderBy(x => x.PersonName));
                    break;
                case Core.SortedBy.Class:
                case Core.SortedBy.Position:
                    ContactListData.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListData.FilteredItems.OrderBy(x => x.PersonPosition));
                    break;
                case Core.SortedBy.NewMessage:
                    ContactListData.FilteredItems = new ObservableCollection<ContactViewModel>(ContactListData.FilteredItems.OrderBy(x => x.HasNewMessages ? 0 : 1));
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
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderBy(x => x.Title));
                    break;
                case Core.SortedBy.Date:
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderByDescending(x => x.SendDate));
                    break;
                case Core.SortedBy.Unread:
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderBy(x => x.WasRead ? 1 : 0));
                    break;
                case Core.SortedBy.Read:
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderBy(x => x.WasRead ? 0 : 1));
                    break;
                case Core.SortedBy.Stared:
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderBy(x => x.IsStared ? 1 : 0));
                    break;
                case Core.SortedBy.Unstared:
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderBy(x => x.IsStared ? 0 : 1));
                    break;
                case Core.SortedBy.Author:
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.Items.OrderBy(x => x.SenderName));
                    break;
            }
        }

        private void GoToCategory()
        {
            MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(Messages[SelectedCategory]);
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
