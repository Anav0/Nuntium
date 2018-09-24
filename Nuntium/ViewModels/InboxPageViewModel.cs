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

        private List<MessageMiniatureViewModel> mRecentlyDeletedMessages = new List<MessageMiniatureViewModel>();

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

        public bool IsDeleting { get; set; }

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

        public ICommand RestoreDeletedMessagesCommand { get; set; }

        #endregion

        #region Constructor

        public InboxPageViewModel()
        {
            var rnd = new Random();

            LoadPupilsCommand = new RelayCommand(LoadPupils);
            LoadTeachersCommand = new RelayCommand(LoadTeachers);
            DeleteSelectedMessagesCommand = new RelayCommand(DeleteSelectedMessages);
            RestoreDeletedMessagesCommand = new RelayCommand(RestoreDeletedMessages);

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

            //Create list for each category
            foreach (var category in (InboxCategoryType[])Enum.GetValues(typeof(InboxCategoryType)))
            {
                Messages.Add(category, new ObservableCollection<MessageMiniatureViewModel>());
            }

            //TODO: delete after frontend is done
            for (int i = 0; i < 4; i++)
            {
                var msg = new MessageMiniatureViewModel
                {
                    Id = i.ToString(),
                    AvatarBackground = ColorHelpers.GenerateRandomColor(),
                    HasAttachments = rnd.Next(0, 100) < 60,
                    SendDate = DateHelper.RandomDate(),
                    SenderName = Faker.Name.FullName(Faker.NameFormats.Standard),
                    Title = Faker.Lorem.Sentence(),
                    Placement = new List<InboxCategoryType>(),
                    WasRead = rnd.Next(0, 100) < 60,
                    MessageSnipit = string.Join(" ", Faker.Lorem.Sentences(10))

                };
                msg.Placement.Add(InboxCategoryType.Inbox);
                msg.OnItemDeleted += OnItemDeleted;
                msg.OnItemStared += OnItemStared;
                msg.Initials = msg.SenderName.GetInitials();

                Messages[InboxCategoryType.Inbox].Add(msg);
            }

            ContactListData.AllItems.Sort((y, x) => string.Compare(y.PersonName, x.PersonName));

            MessageListData.SelectedItemChanged += OnSelectedItemChanged;

            LoadTeachers();
            GoToCategory();
            SortMessages();
        }



        #endregion

        #region Event handlers

        private void OnSelectedItemChanged(object sender, EventArgs e)
        {
            if (MessageListData.Selected == null)
                return;

            if (MessageListData.SelectedItems.Count > 1)
                return;

            MessageListData.Selected.WasRead = true;
        }

        private void OnItemDeleted(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageListData.Items.Remove(item);
            });

            MoveMessageDownCategoryHierarchy(item);

            mRecentlyDeletedMessages.Clear();
            mRecentlyDeletedMessages.Add(item);

            DisplayReverseDeletionPopup();
        }

        private void OnItemStared(object sender, EventArgs e)
        {
            if (!(sender is MessageMiniatureViewModel item))
                return;

            if (item.IsStared)
            {
                Messages[InboxCategoryType.Stared].Add(item);
                item.Placement.Add(InboxCategoryType.Stared);
            }
            else
            {
                Messages[InboxCategoryType.Stared].Remove(item);
                item.Placement.Remove(InboxCategoryType.Stared);
            }

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

        private void DeleteSelectedMessages()
        {
            if (MessageListData.SelectedItems.Count == 0)
                return;

            new Thread(() =>
            {

                var itemsToDelete = new List<MessageMiniatureViewModel>(MessageListData.SelectedItems);
                var shownItems = new List<MessageMiniatureViewModel>(MessageListData.Items);
                mRecentlyDeletedMessages = new List<MessageMiniatureViewModel>(itemsToDelete);

                itemsToDelete.ForEach
                (x =>
                {
                    x.AnimateOut = true;
                    MoveMessageDownCategoryHierarchy(x);
                    shownItems.Remove(x);
                });

                Thread.Sleep(itemsToDelete[0].AnimateOutTimeSpan);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageListData.Items = new ObservableCollection<MessageMiniatureViewModel>(shownItems);
                });


                DisplayReverseDeletionPopup();

            }).Start();
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

        private void RestoreDeletedMessages()
        {
            IsDeleting = false;

            mRecentlyDeletedMessages.ForEach(x =>
            {

                //send message back to category from which it came
                foreach (var place in x.PrevPlacement)
                {
                    Messages[place].Add(x);
                }

                foreach (var place in x.Placement)
                {
                    Messages[place].Remove(x);
                }

                //swap message location
                x.Placement = new List<InboxCategoryType>(x.PrevPlacement);
                x.PrevPlacement.Clear();
            });

            GoToCategory();
            SortMessages();
            SelectMessages();
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

        private void MoveMessageDownCategoryHierarchy(MessageMiniatureViewModel item)
        {
            if (item == null)
                return;

            var places = new List<InboxCategoryType>(item.Placement);

            foreach (var placement in places)
            {

                switch (placement)
                {
                    default:
                        //remove item from where it is now...
                        Messages[placement].Remove(item);

                        //add item to deleted items list
                        if(!Messages[InboxCategoryType.Deleted].Contains(item))
                            Messages[InboxCategoryType.Deleted].Add(item);

                        item.Placement.Remove(placement);
                        item.PrevPlacement.Add(placement);
                        item.Placement.Add(InboxCategoryType.Deleted);
                        break;

                    case InboxCategoryType.Deleted:
                        Messages[placement].Remove(item);
                        break;
                }

            }

           

        }

        private void DisplayReverseDeletionPopup()
        {
            IsDeleting = true;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Elapsed += (s, e) =>
            {
                IsDeleting = false;
                timer.Stop();
            };
            timer.Start();
        }

        #endregion
    }
}
