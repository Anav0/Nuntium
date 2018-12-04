using Microsoft.Win32;
using Ninject;
using Nuntium.Core;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using System.Windows;

namespace Nuntium
{
    public class InsertSubmenuViewModel : BaseViewModel
    {

        public InsertSubmenuViewModel(FlowDocument document, Point popupPlacement, TextSelection selectedText)
        {
            InsertImageCommand = new RelayCommandWithParameter((param) => { InsertImage(document); });
            InsertLinkCommand = new RelayCommandWithParameter((param) => { InsertLink(selectedText, popupPlacement); });
            InsertFileCommand = new RelayCommand(() => { IoC.Kernel.Get<AttachmentsSectionViewModel>().InsertFileFromDialog(); });
        }

        #region Public commands

        public ICommand InsertFileCommand { get; private set; }

        public ICommand InsertImageCommand { get; private set; }

        public ICommand InsertLinkCommand { get; private set; }

        #endregion

        private void InsertImage(FlowDocument document)
        {
            if (document == null)
                return;

            var dialog = new OpenFileDialog();
            dialog.Title = "Select image to insert";
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (dialog.ShowDialog() == true)
            {
                BitmapImage bimage = new BitmapImage();
                bimage.BeginInit();
                bimage.UriSource = new Uri(dialog.FileName, UriKind.Absolute);
                bimage.EndInit();

                var image = new Image { Source = bimage };
                document.InsertAdornedImage(image);
            }
        }

        private void InsertLink(TextSelection selectedText, Point popupPlacement)
        {
            var insertLinkPopup = new InsertLinkPopup();

            var existingLinks = selectedText.GetHyperlinksFromSelection();

            if (existingLinks == null || existingLinks.Count > 1)
                return;

            if (existingLinks.Count == 1)
                insertLinkPopup.Link = existingLinks[0]?.NavigateUri?.ToString();

            insertLinkPopup.TextToDisplay = selectedText.Text;

            var popup = new OffsetPopupFactory().CreatePopupOnPoint(popupPlacement);

            insertLinkPopup.AcceptCommand = new RelayCommand(() =>
            {
                if (string.IsNullOrEmpty(insertLinkPopup.Link) || string.IsNullOrEmpty(insertLinkPopup.TextToDisplay))
                    return;

                selectedText.Text = insertLinkPopup.TextToDisplay.RemoveWhitespace();

                new BasicHyperLinkFactory().CreateHyperLinkOnTopOfSelectedText(selectedText, insertLinkPopup.Link);

                popup.IsOpen = false;

            });

            popup.Child = insertLinkPopup;
            popup.IsOpen = true;
        }

    }
}
