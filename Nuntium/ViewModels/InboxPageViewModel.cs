using Nuntium.Core;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Nuntium
{
    public class InboxSectionViewModel : BaseViewModel
    {

        #region Private Members

        private SortingOption mContactsSortedBy;

        private InboxCategoryType mSelectedCategory;

        private SortingOption mMessagesSortedBy;

        private bool? mSelectionMode = false;

        #endregion

        #region Public Properties

        public ContactsListViewModel ContactListVM { get; set; }

        public MessageMiniatureListViewModel MessageListVM { get; set; }

        public ObservableCollection<SortingOption> ContactsSortingOptions { get; set; }

        public ObservableCollection<SortingOption> MessagesSortingOptions { get; set; }

        public Dictionary<InboxCategoryType, ObservableCollection<MessageMiniatureViewModel>> Messages { get; set; }

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

        public InboxSectionViewModel(IEmailService emailService, IEventAggregator eventAggregator, ICatalogService catalogService, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            Messages = new Dictionary<InboxCategoryType, ObservableCollection<MessageMiniatureViewModel>>();
            InitializeCommands();
            InitializeSortingOptions();
            InitializeSubordinateViewModels(catalogService, eventAggregator);
            HookToEvents(eventAggregator, catalogService);

            foreach (var category in (InboxCategoryType[])Enum.GetValues(typeof(InboxCategoryType)))
            {
                Messages.Add(category, new ObservableCollection<MessageMiniatureViewModel>());
            }

            GetEmailsFromService(emailService, eventAggregator, catalogService, editor, addressSectionViewModel);
            LoadTeachers();
            GoToSelectedCategory();
            SortMessages();
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

        private void DeleteSelectedMessages()
        {
            for (int i = MessageListVM.SelectedItems.Count - 1; i >= 0; i--)
            {
                var email = MessageListVM.SelectedItems[i];

                MoveEmailTo(email, InboxCategoryType.Deleted);

                //If message gets deleted remove it's email details screen
                if (email.Placement == InboxCategoryType.Deleted)
                    ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.Blank);

            }
        }

        private void ArchiveSelectedMessages()
        {
            if (SelectedCategory == InboxCategoryType.Archive)
                return;

            for (int i = MessageListVM.SelectedItems.Count - 1; i >= 0; i--)
            {
                MoveEmailTo(MessageListVM.SelectedItems[i], InboxCategoryType.Archive);
            }
        }

        private void StarSelectedMessages()
        {
            foreach (var email in MessageListVM.SelectedItems)
            {
                MoveEmailTo(email, InboxCategoryType.Stared);
            }
        }

        private void ShowTextEditor()
        {
            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor, new TextEditorViewModel());
        }

        #endregion

        #region Private Methods

        private void HookToEvents(IEventAggregator eventAggregator, ICatalogService catalogService)
        {

            eventAggregator.GetEvent<EmailDeletedEvent>().Subscribe((emailId) =>
            {
                var email = MessageListVM.Items.First(x => x.Id == emailId);
                MoveEmailTo(email, InboxCategoryType.Deleted);
            });

            eventAggregator.GetEvent<EmailArchivedEvent>().Subscribe((emailId) =>
            {
                var email = MessageListVM.Items.First(x => x.Id == emailId);
                MoveEmailTo(email, InboxCategoryType.Archive);
            });

            eventAggregator.GetEvent<EmailStaredEvent>().Subscribe((emailId) =>
            {
                var email = MessageListVM.Items.First(x => x.Id == emailId);
                MoveEmailTo(email, InboxCategoryType.Stared);
            });

            eventAggregator.GetEvent<MoveEmailToCatalog>().Subscribe((emailCatalogPayload) =>
            {
                MessageMiniatureViewModel email = null;

                var catalog = catalogService.GetCatalogById(emailCatalogPayload.CatalogId);

                foreach (var group in Messages)
                {
                    foreach (var eml in group.Value)
                    {
                        if (eml.Id == emailCatalogPayload.EmailId)
                        {
                            email = eml;
                            break;
                        }
                    }
                }

                if (email == null) MessageBox.Show("Email not found", "MessageMiniatureListViewModel", MessageBoxButton.OK, MessageBoxImage.Error);
                if (catalog == null) MessageBox.Show("Catalog not found", "MessageMiniatureListViewModel", MessageBoxButton.OK, MessageBoxImage.Error);

                MoveEmailTo(email, catalog.Category);

                //If message gets deleted remove it's email details screen
                if (catalog.Category == InboxCategoryType.Deleted && catalog.Category == email.Placement)
                    ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.Blank);

            }, true);
        }

        private void GetEmailsFromService(IEmailService emailService, IEventAggregator eventAggregator, ICatalogService catalogService, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            foreach (var email in emailService.GetAllEmails())
            {
                var msg = new MessageMiniatureViewModel(
                    email.Id,
                    eventAggregator,
                    emailService,
                    catalogService,
                    editor,
                    addressSectionViewModel
                    )
                {
                    AvatarBackground = ColorHelpers.GenerateRandomColor(),
                    HasAttachments = email.Attachments.Count > 0 ? true : false,
                    SendDate = email.SendDate,
                    SenderName = email.SenderName,
                    Subject = email.Subject,
                    Placement = InboxCategoryType.Inbox,
                    Message = email.Message,
                    WasRead = email.WasRead

                };

                Messages[InboxCategoryType.Inbox].Add(msg);
            }
        }

        private void InitializeSubordinateViewModels(ICatalogService catalogService, IEventAggregator eventAggregator)
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

            MessageListVM = new MessageMiniatureListViewModel(eventAggregator, catalogService);

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
            ShowTextEditorCommand = new RelayCommand(ShowTextEditor);
            //TODO: Prepere proper message restoration
            RestoreDeletedMessagesCommand = new RelayCommand(() => { });
            StarSelectedMessagesCommand = new RelayCommand(StarSelectedMessages);
            DeleteSelectedMessagesCommand = new RelayCommand(() =>
            {
                DeleteSelectedMessages();
                GoToSelectedCategory();
                SortMessages();
            });
            ArchiveSelectedMessagesCommand = new RelayCommand(() =>
            {
                ArchiveSelectedMessages();
                GoToSelectedCategory();
                SortMessages();
            });
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

        public void MoveEmailTo(MessageMiniatureViewModel email, InboxCategoryType catalog)
        {
            //Is we archiving email mark it appropriately
            if (catalog == InboxCategoryType.Archive)
            {
                email.IsArchived = true;

            }
            else
                email.IsArchived = false;

            if (catalog == InboxCategoryType.Stared)
            {
                if (email.IsStared)
                {
                    email.IsStared = false;
                    Messages[InboxCategoryType.Stared].Remove(email);
                    return;
                }

                email.IsStared = true;
                Messages[catalog].Add(email);
                return;
            }

            //If email is going to be permamently deleted, delete it from stared emails also
            if (catalog == InboxCategoryType.Deleted && email.Placement == InboxCategoryType.Deleted)
            {
                Messages[InboxCategoryType.Stared].Remove(email);
                Messages[email.Placement].Remove(email);
                return;
            }

            var tmp = email.Placement;
            email.PrevPlacement = email.Placement;
            email.Placement = catalog;

            Messages[email.PrevPlacement].Remove(email);
            Messages[catalog].Add(email);
        }

        #endregion

    }
}
