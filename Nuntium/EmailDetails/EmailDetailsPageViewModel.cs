﻿
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
        #region Public Properties

        public string Html { get; set; }

        public string SenderName { get; set; }

        public string QuickReply { get; set; }

        public ObservableCollection<AttachFileViewModel> AttachementsList { get; set; }

        public string AvatarBockground { get; set; }

        #endregion

        private string mEmailId;

        public EmailDetailsPageViewModel()
        {

        }

        public EmailDetailsPageViewModel(string EmailId, IEmailService emailService, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel, string avatarBackground)
        {
            var email = emailService.GetEmailById(EmailId);

            this.mEmailId = EmailId;

            this.AvatarBockground = avatarBackground;

            this.Html = email.Message; //GetHtmlFromLink("http://c0185784a2b233b0db9b-d0e5e4adc266f8aacd2ff78abb166d77.r51.cf2.rackcdn.com/v1_templates/template_02.html");

            this.SenderName = email.SenderName;

            InitializeCommands(email, editor, addressSectionViewModel);

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

        private void InitializeCommands(Email email, CustomRichTextBox editor, AddressSectionViewModel addressSectionViewModel)
        {
            DeleteCommand = new RelayCommand(() =>
            {
                //TODO: Change to event publisher
                //inboxPageViewModel.DeleteMessage(DisplayedMessageVM, new EventArgs());

                IoC.Kernel.Get<IEventAggregator>().GetEvent<EmailDeletedEvent>().Publish(email.Id);

                ConstantViewModels.Instance.ApplicationViewModelInstance.GoToPage(ApplicationPage.Blank);
            });

            ReplyCommand = new RelayCommand(() => { Reply(email, editor, addressSectionViewModel); });

            ReplyToAllCommand = new RelayCommand(() => { ReplyToAll(email, editor, addressSectionViewModel); });
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
