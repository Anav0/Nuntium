
using Ninject;
using Nuntium.Core;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;

namespace Nuntium
{
    public class EmailDetailsPageViewModel : BaseViewModel
    {

        private IEventAggregator mEventAggregator;

        #region Public Properties

        public string Html { get; set; }

        public string SenderName { get; set; }

        public string QuickReply { get; set; }

        public ObservableCollection<AttachFileViewModel> AttachementsList { get; set; }

        public string AvatarBockground { get; set; } = "#4c6ef5";

        public bool IsShowMoreOptionsMenuVisible { get; set; }

        public ObservableCollection<MoreOptionsItemViewModel> MoreOptionsItems { set; get; }

        public ObservableCollection<MoreOptionsItemViewModel> MoveEmailOptionsItems { set; get; }
        
        public ComplexMenuViewModel ComplexMenuVM { get; set; }

        #endregion


        public EmailDetailsPageViewModel()
        {

        }

        public EmailDetailsPageViewModel(
            string EmailId, 
            IEmailService emailService, 
            CustomRichTextBox editor, 
            AddressSectionViewModel addressSectionViewModel, 
            string avatarBackground, 
            IEventAggregator eventAggregator,
            ICatalogService catalogService)
        {
            mEventAggregator = eventAggregator;

            var email = emailService.GetEmailById(EmailId);

            InitializeFields(avatarBackground, email);

            InitializeCommands(email, editor, addressSectionViewModel);

            InitializeLists(catalogService, email);

            ComplexMenuVM = new ComplexMenuViewModel
            {
                Items = MoreOptionsItems,
            };

        }


        #region Public Commands

        public ICommand DownloadAllAttachmentsCommand { get; set; }

        public ICommand ReplyCommand { get; set; }

        public ICommand ReplyToAllCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand SendQuickReplyCommand { get; set; }

        public ICommand AddAttachmentCommand { get; set; }

        public ICommand ShowMoreOptionsCommand { get; set; }

        #endregion

        private void InitializeCommands(Email email, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            DeleteCommand = new RelayCommand(() =>
            {
                mEventAggregator.GetEvent<EmailDeletedEvent>().Publish(email.Id);

                ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.Blank);
            });

            ReplyCommand = new RelayCommand(() => { Reply(email, editor, addressSectionViewModel); });

            ReplyToAllCommand = new RelayCommand(() => { ReplyToAll(email, editor, addressSectionViewModel); });

            ShowMoreOptionsCommand = new RelayCommand(() => { IsShowMoreOptionsMenuVisible ^= true; });
        }

        private void InitializeLists(ICatalogService catalogService, Email email)
        {
            AttachementsList = new ObservableCollection<AttachFileViewModel>();

            MoveEmailOptionsItems = new ObservableCollection<MoreOptionsItemViewModel>();

            foreach (var catalog in catalogService.GetAllCatalogs())
            {
                MoveEmailOptionsItems.Add(new MoreOptionsItemViewModel
                {
                    Icon = catalog.Icon,
                    Text = catalog.DisplayName,
                    Command = new RelayCommand(() =>
                    {
                        var payload = new EmailCatalogPayload(email.Id, catalog.Id);
                        mEventAggregator.GetEvent<MoveEmailToCatalog>().Publish(payload);
                    }),
                });
            }

            MoreOptionsItems = new ObservableCollection<MoreOptionsItemViewModel>
            {
                new MoreOptionsItemViewModel
                {
                    Text = "Mark as unread",
                    Command = new RelayCommand(() =>
                    {
                        mEventAggregator.GetEvent<MarkEmailAsUnread>().Publish(email.Id);
                    }),
                    Icon = IconType.Envelope
                },
                new MoreOptionsItemViewModel
                {
                    Text = "Move",
                    Command = new RelayCommand(()=>
                    {
                        ComplexMenuVM.SwapContent(MoveEmailOptionsItems);
                    }),
                    Icon = IconType.Suitcase
                },
                new MoreOptionsItemViewModel
                {
                    Text = "Previous",
                    Command = new RelayCommand(() => {var test = 2+2; }),
                    Icon = IconType.CaretLeft
                },
                new MoreOptionsItemViewModel
                {
                    Text = "Next",
                    Command = new RelayCommand(()=>{var test = 2+2; }),
                    Icon = IconType.CaretRight
                },

                new MoreOptionsItemViewModel
                {
                    Text = "Search",
                    Command = new RelayCommand(()=>{var test = 2+2; }),
                    Icon = IconType.Search
                },

                new MoreOptionsItemViewModel
                {
                    Text = "Save as",
                    Command = new RelayCommand(()=>{var test = 2+2; }),
                    Icon = IconType.Save
                },

                new MoreOptionsItemViewModel
                {
                    Text = "Print",
                    Command = new RelayCommand(()=>{var test = 2+2; }),
                    Icon = IconType.Print
                },
            };
        }

        private void InitializeFields(string avatarBackground, Email email)
        {
            AvatarBockground = avatarBackground;

            Html = email.Message; //GetHtmlFromLink("http://c0185784a2b233b0db9b-d0e5e4adc266f8aacd2ff78abb166d77.r51.cf2.rackcdn.com/v1_templates/template_02.html");

            SenderName = email.SenderName;
        }

        private void ReplyToAll(Email email, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            addressSectionViewModel.ToEmailsList.Clear();

            AddInformationAboutReplyToEditor(email, editor);
            AddInformationAboutReplyToAddressSection(email, addressSectionViewModel, email.ToAddresses);

            //Go to TextEditor
            //TODO: After creating new instance of TextEditor CustomRichTextBox gets rebind. It preventes us from working on the same object
            //find a way to fix it
            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor, new TextEditorViewModel());
        }

        private void Reply(Email email, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            addressSectionViewModel.ToEmailsList.Clear();

            AddInformationAboutReplyToEditor(email, editor);
            AddInformationAboutReplyToAddressSection(email, addressSectionViewModel);

            //Go to TextEditor
            //TODO: After creating new instance of TextEditor CustomRichTextBox gets rebind. It preventes us from working on the same object
            //find a way to fix it
            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor, new TextEditorViewModel());
        }

        private void AddInformationAboutReplyToAddressSection(Email email, AddressSectionViewModel addressSectionViewModel, List<string> to = null)
        {
            addressSectionViewModel.Topic = "RE: " + email.Subject;

            addressSectionViewModel.ToEmailsList.Add(new MailWrapperViewModel
            {
                Address = email.Address,
            });

            if (to == null) return;

            foreach (var address in to)
            {
                //TODO: add check so it would not reply to login user's email address
                var wrapper = new MailWrapperViewModel
                {
                    Address = address,
                };

                wrapper.DeleteCommand = new RelayCommand(() =>
                {
                    addressSectionViewModel.ToEmailsList.Remove(wrapper);
                });

                addressSectionViewModel.ToEmailsList.Add(wrapper);
            }

        }

        private void AddInformationAboutReplyToEditor(Email email, CustomRichTextBox editor)
        {
            var toWhom = email.Address;
            foreach (var address in email.ToAddresses)
            {
                //TODO: add check for logged user's email address
                toWhom += address + " ";
            }

            editor.Document.Blocks.Add(new Paragraph(new Run("From: " + email.Address)));
            editor.Document.Blocks.Add(new Paragraph(new Run("Send: " + email.SendDate.ToString())));
            editor.Document.Blocks.Add(new Paragraph(new Run("To: " + toWhom)));
            editor.Document.Blocks.Add(new Paragraph(new Run("Subject: " + email.Subject)));
            editor.Document.Blocks.Add(new Paragraph(new Run(email.Message)));
        }

        private string GetHtmlFromLink(string link)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                var output = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return output;
            }

            return response.StatusDescription;
        }
    }

}
