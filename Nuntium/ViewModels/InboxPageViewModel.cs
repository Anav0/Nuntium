using Nuntium.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Nuntium
{
    public class InboxPageViewModel : BaseViewModel
    {
        #region Public properties

        public ContactsListViewModel ContactListData { get; set; }

        public MessageMiniatureListViewModel MessageListData { get; set; }

        public ObservableCollection<SortingOption> ContactsSortingOptions { get; set; }

        public ObservableCollection<SortingOption> MessagesSortingOptions { get; set; }

        private SortingOption _ContactsSortedBy;
        public SortingOption ContactsSortedBy
        {
            get => _ContactsSortedBy;
            set
            {
                _ContactsSortedBy = value;

                SortContacts();
            }
        }

        private InboxCategoryType _SelectedCategory;
        public InboxCategoryType SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                if (_SelectedCategory == value)
                    return;

                _SelectedCategory = value;

                GoToCategory();

            }
        }

        private SortingOption _MessagesSortedBy;
        public SortingOption MessagesSortedBy
        {
            get => _MessagesSortedBy;
            set
            {
                _MessagesSortedBy = value;

                SortMessages();
            }
        }

        public int NumberOfInboxMessages { get; set; }

        public int NumberOfDraftMessages { get; set; }

        public int NumberOfSentMessages { get; set; }

        public int NumberOfDeletedMessages { get; set; }

        private bool? _SelectionMode;
        public bool? SelectionMode
        {
            get => _SelectionMode;

            set
            {
                if (_SelectionMode != value)
                    _SelectionMode = value;

            }
        }

        #endregion

        #region Public Commands

        public ICommand LoadTeachersCommand { get; set; }

        public ICommand LoadPupilsCommand { get; set; }

        public ICommand DeleteSelectedMessagesCommand { get; set; }

        #endregion

        #region Constructor

        public InboxPageViewModel()
        {
            var rnd = new Random();

            LoadPupilsCommand = new RelayCommand(LoadPupils);
            LoadTeachersCommand = new RelayCommand(LoadTeachers);
            DeleteSelectedMessagesCommand = new RelayCommand(DeleteSelectedMessages);

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

            MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>();

            //TODO: delete after frontend is done
            for (int i = 0; i < 150; i++)
            {
                var msg = new MessageMiniatureViewModel
                {
                    AvatarBackground = ColorHelpers.GenerateRandomColor(),
                    HasAttachments = rnd.Next(0, 100) < 60,
                    SendDate = DateHelper.RandomDate(),
                    SenderName = Faker.Name.FullName(Faker.NameFormats.Standard),
                    Title = Faker.Lorem.Sentence(),
                    Placement = InboxCategoryType.Inbox,
                    WasRead = rnd.Next(0, 100) < 60,
                    MessageSnipit = string.Join(" ", Faker.Lorem.Sentences(10))

                };
                msg.OnItemDeleted += OnItemDeleted;
                msg.Initials = msg.SenderName.GetInitials();
                MessageListData.AllItems.Add(msg);
            }

            MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.AllItems);

            ContactListData.AllItems.Sort((y, x) => string.Compare(y.PersonName, x.PersonName));

            MessageListData.SelectedItemChanged += SelectedItemChanged;

            NumberOfInboxMessages = MessageListData.FilteredItems.Count;

            LoadTeachers();
        }

        private void OnItemDeleted(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageListData.FilteredItems.Remove(item);
            });

            PlaceInCategory(item);
        }

        #endregion

        #region Event handlers

        private void SelectedItemChanged(object sender, EventArgs e)
        {
            if (MessageListData.Selected == null)
                return;

            MessageListData.Selected.WasRead = true;
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

        private void PlaceInCategory(MessageMiniatureViewModel item)
        {
            if (item == null)
                return;

            switch (item.Placement)
            {
                case InboxCategoryType.Inbox:
                    item.Placement = InboxCategoryType.Deleted;
                    NumberOfDeletedMessages++;
                    NumberOfInboxMessages--;
                    break;

                case InboxCategoryType.Deleted:
                    MessageListData.AllItems.Remove(item);
                    NumberOfDeletedMessages--;
                    break;

                case InboxCategoryType.Sent:
                    item.Placement = InboxCategoryType.Deleted;
                    NumberOfDeletedMessages++;
                    NumberOfSentMessages--;
                    break;

                case InboxCategoryType.Drafts:
                    item.Placement = InboxCategoryType.Deleted;
                    NumberOfDeletedMessages++;
                    NumberOfDraftMessages--;
                    break;
            }
        }

        private void DeleteSelectedMessages()
        {
            if (MessageListData.SelectedItems.Count == 0)
                return;

            new Thread(() =>
            {
                var itemsToDelete = new List<MessageMiniatureViewModel>(MessageListData.SelectedItems);
                var shownItems = new List<MessageMiniatureViewModel>(MessageListData.FilteredItems);

                itemsToDelete.ForEach
                (x =>
                {
                    x.AnimateOut = true;
                    PlaceInCategory(x);
                    shownItems.Remove(x);
                });

                Thread.Sleep(itemsToDelete[0].AnimateOutTimeSpan);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(shownItems);
                });

            }).Start();
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
                    MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.FilteredItems.OrderBy(x => x.Title));
                    break;
                case Core.SortedBy.Date:
                    MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.FilteredItems.OrderByDescending(x => x.SendDate));
                    break;
                case Core.SortedBy.Unread:
                    MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.FilteredItems.OrderBy(x => x.WasRead ? 1 : 0));
                    break;
                case Core.SortedBy.Author:
                    MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.FilteredItems.OrderBy(x => x.SenderName));
                    break;
            }
        }

        private void GoToCategory()
        {
            MessageListData.FilteredItems = new ObservableCollection<MessageMiniatureViewModel>(MessageListData.AllItems.FindAll(x => x.Placement == SelectedCategory));
        }

        #endregion
    }
}
