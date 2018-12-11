
using Ninject;
using Nuntium.Core;
using System;
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
        #region Public Properties

        public string Html { get; set; }

        public string SenderName { get; set; }

        public string QuickReply { get; set; }

        public ObservableCollection<AttachFileViewModel> AttachementsList { get; set; }

        #endregion

        private string mEmailId;

        public EmailDetailsPageViewModel()
        {
           
        }

        public EmailDetailsPageViewModel(string EmailId, InboxPageViewModel inboxPageViewModel, IEmailService emailService, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            this.mEmailId = EmailId;
            var email = emailService.GetEmailById(EmailId);

            this.Html = email.Message; //GetHtmlFromLink("http://c0185784a2b233b0db9b-d0e5e4adc266f8aacd2ff78abb166d77.r51.cf2.rackcdn.com/v1_templates/template_02.html");

            this.SenderName = email.SenderName;

            InitializeCommands(editor, addressSectionViewModel, email);

            AttachementsList = new ObservableCollection<AttachFileViewModel>();

        }
       

        #region Public Commands

        public ICommand DownloadAllAttachmentsCommand { get; set; }

        public ICommand ReplyCommand { get; set; }

        public ICommand ReplyToAllCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand SendQuickReplyCommand { get; set; }

        public ICommand AddAttachmentCommand { get; set; }

        #endregion

        private void InitializeCommands(CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel, Email email)
        {
            DeleteCommand = new RelayCommand(() =>
            {
                //TODO: Change to event publisher
                //inboxPageViewModel.DeleteMessage(DisplayedMessageVM, new EventArgs());

                ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.Blank);
            });

            ReplyCommand = new RelayCommandWithParameter((param) => { Reply(email, editor, addressSectionViewModel); });
        }

        private void Reply(Email email, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {

            ConstantViewModels.Instance.AddressSectionVM.ToEmailsList.Clear();

            //Go to TextEditor
            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor, new TextEditorViewModel());

            AddInformationAboutEmailToEditor(email, editor, addressSectionViewModel);

            var wrapper = new MailWrapperViewModel
            {
                Address = ConstantViewModels.Instance.EmailServiceInstance.GetEmailById(mEmailId).Address,
            };

            wrapper.DeleteCommand = new RelayCommand(() =>
            {
                ConstantViewModels.Instance.AddressSectionVM.ToEmailsList.Remove(wrapper);
            });

            ConstantViewModels.Instance.AddressSectionVM.ToEmailsList.Add(wrapper);
        }

        private void AddInformationAboutEmailToEditor(Email email, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            addressSectionViewModel.Topic = "RE: " + email.Subject;

            var to = "";
            email.ToAddresses.ForEach(x => to += x + ";");

            editor.Document.Blocks.Add(new Paragraph(new Run("From: " + email.Address)));
            editor.Document.Blocks.Add(new Paragraph(new Run("Send: " + email.SendDate.ToString())));
            editor.Document.Blocks.Add(new Paragraph(new Run("To: " + to)));
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
