using Ninject;
using Nuntium.Core;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for AddressInputBox.xaml
    /// </summary>
    public partial class AddressInputBox : UserControl
    {
        public AddressInputBox()
        {
            //Important to initialize this DP here otherwise it's value will be bound to any other AddressInputInstances
            Addresses = new ObservableCollection<MailWrapperViewModel>();
            InitializeComponent();
        }

        #region Purpose DP

        public AddressCategory Purpose
        {
            get { return (AddressCategory)GetValue(PurposeProperty); }
            set { SetValue(PurposeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Purpose.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PurposeProperty =
            DependencyProperty.Register("Purpose", typeof(AddressCategory), typeof(AddressInputBox), new PropertyMetadata());

        #endregion

        #region Address DP

        public string  Address
        {
            get { return (string )GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Address.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string ), typeof(AddressInputBox), new PropertyMetadata(""));

        #endregion

        #region Addresses DP

        public ObservableCollection<MailWrapperViewModel> Addresses
        {
            get { return (ObservableCollection<MailWrapperViewModel>)this.GetValue(AddressesProperty); }
            set { this.SetValue(AddressesProperty, value); }
        }

        public static readonly DependencyProperty AddressesProperty =
            DependencyProperty.Register("Addresses",
            typeof(ObservableCollection<MailWrapperViewModel>), typeof(AddressInputBox),
            new FrameworkPropertyMetadata());

        #endregion

        #region IsRemovable

        public bool IsRemovable
        {
            get { return (bool)GetValue(IsRemovableProperty); }
            set { SetValue(IsRemovableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRemovable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRemovableProperty =
            DependencyProperty.Register("IsRemovable", typeof(bool), typeof(AddressInputBox), new PropertyMetadata(true));

        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox box))
                return;

            if (string.IsNullOrEmpty(box.Text))
                return;

            var lastChar = box.Text[box.Text.Length - 1];

            if (lastChar == ';')
            {
                ParseAddress(box, true);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox box))
                return;

            if (string.IsNullOrEmpty(box.Text))
                return;

            ParseAddress(box);
        }

        private void ParseAddress(TextBox box, bool excludeLastCharacter = false)
        {
            //check if email entered is valid if not return (using microsoft method).
            if (!(new EmailAddressAttribute().IsValid(box.Text)))
                return;

            if (Addresses == null)
                Addresses = new ObservableCollection<MailWrapperViewModel>();

            var lastChar = 0;

            if(excludeLastCharacter)
            {
                lastChar = 1;
            }

            var adr = box.Text.Remove(box.Text.Length - 1, lastChar);

            var wrapperVM = new MailWrapperViewModel
            {
                Address = box.Text.Remove(box.Text.Length - 1, lastChar).RemoveWhitespace(),
            };

            wrapperVM.DeleteCommand = new RelayCommand (() =>
            {
                Addresses.Remove(wrapperVM);
                RemoveBasedOnPurpose(wrapperVM);
            });

            AddBasedOnPurpose(wrapperVM);
            Addresses.Add(wrapperVM);

            box.Text = "";
        }

        private void AddBasedOnPurpose(MailWrapperViewModel wrapper)
        {
            switch(Purpose)
            {
                case AddressCategory.BCC:
                    IoC.Kernel.Get<AddressSectionViewModel>().BCCEmailsList.Add(wrapper);
                    break;
                case AddressCategory.CC:
                    IoC.Kernel.Get<AddressSectionViewModel>().CCEmailsList.Add(wrapper);
                    break;
                case AddressCategory.To:
                    IoC.Kernel.Get<AddressSectionViewModel>().ToEmailsList.Add(wrapper);
                    break;
            }
        }

        private void RemoveBasedOnPurpose(MailWrapperViewModel wrapper)
        {
            switch (Purpose)
            {
                case AddressCategory.BCC:
                    IoC.Kernel.Get<AddressSectionViewModel>().BCCEmailsList.Remove(wrapper);
                    break;
                case AddressCategory.CC:
                    IoC.Kernel.Get<AddressSectionViewModel>().CCEmailsList.Remove(wrapper);
                    break;
                case AddressCategory.To:
                    IoC.Kernel.Get<AddressSectionViewModel>().ToEmailsList.Remove(wrapper);
                    break;
            }
        }
    }
}
