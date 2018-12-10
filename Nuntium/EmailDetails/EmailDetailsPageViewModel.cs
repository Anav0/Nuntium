
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

        public MessageMiniatureViewModel DisplayedMessageVM { get; set; }

        #endregion

        public EmailDetailsPageViewModel()
        {
           
        }

        public EmailDetailsPageViewModel(MessageMiniatureViewModel DisplayedMessageVM)
        {
            this.DisplayedMessageVM = DisplayedMessageVM;

            this.Html = DisplayedMessageVM.Message; //GetHtmlFromLink("http://c0185784a2b233b0db9b-d0e5e4adc266f8aacd2ff78abb166d77.r51.cf2.rackcdn.com/v1_templates/template_02.html");

            this.SenderName = DisplayedMessageVM.SenderName;

            DeleteCommand = new RelayCommand(() =>
            {
                ConstantViewModels.Instance.InboxPageVM.DeleteMessage(DisplayedMessageVM, new System.EventArgs());

                ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(Core.ApplicationPage.Blank);
            });

            ReplyCommand = new RelayCommand(Reply);

            AttachementsList = new ObservableCollection<AttachFileViewModel>();
            //{
            //    new AttachFileViewModel
            //    {
            //        FileName = "cat1.jpg",
            //        FileSize = 3058506,
            //    },

            //    new AttachFileViewModel
            //    {
            //        FileName = "cat2.jpg",
            //        FileSize = 5068576,
            //    },
            //};
        }

        #region Public Commands

        public ICommand DownloadAllAttachmentsCommand { get; set; }

        public ICommand ReplyCommand { get; set; }

        public ICommand ReplyToAllCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand SendQuickReplyCommand { get; set; }

        public ICommand AddAttachmentCommand { get; set; }

        #endregion

        private void Reply()
        {

            ConstantViewModels.Instance.AddressSectionVM.ToEmailsList.Clear();

            //Go to TextEditor
            ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.TextEditor, new TextEditorViewModel());

            AddInformationAboutPrevMessageToTextEditor();

            var wrapper = new MailWrapperViewModel
            {
                Address = ConstantViewModels.Instance.EmailServiceInstance.GetEmailById(DisplayedMessageVM.Id).Address,
            };

            wrapper.DeleteCommand = new RelayCommand(() =>
            {
                ConstantViewModels.Instance.AddressSectionVM.ToEmailsList.Remove(wrapper);
            });

            ConstantViewModels.Instance.AddressSectionVM.ToEmailsList.Add(wrapper);
        }

        private void AddInformationAboutPrevMessageToTextEditor()
        {
            ConstantViewModels.Instance.AddressSectionVM.Topic = "RE: " + DisplayedMessageVM.Title;
            var editor = IoC.Kernel.Get<CustomRichTextBox>();
            var document = editor.Document;
            var email = ConstantViewModels.Instance.EmailServiceInstance.GetEmailById(DisplayedMessageVM.Id);
            var to = "";
            email.ToAddresses.ForEach(x => to += x + ";");

            document.Blocks.Add(new Paragraph(new Run("From: " + email.Address)));
            document.Blocks.Add(new Paragraph(new Run("Send: " + email.SendDate.ToString())));
            document.Blocks.Add(new Paragraph(new Run("To: " + to)));
            document.Blocks.Add(new Paragraph(new Run("Subject: " + email.Subject)));
            document.Blocks.Add(new Paragraph(new Run(DisplayedMessageVM.Message)));
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
