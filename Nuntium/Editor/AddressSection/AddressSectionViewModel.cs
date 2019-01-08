
using Nuntium.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Nuntium
{
    public class AddressSectionViewModel : BaseViewModel
    {
        public ObservableCollection<MailWrapperViewModel> FromEmailsList { get; set; }

        public ObservableCollection<MailWrapperViewModel> ToEmailsList { get; set; }

        public ObservableCollection<MailWrapperViewModel> CCEmailsList { get; set; }

        public ObservableCollection<MailWrapperViewModel> BCCEmailsList { get; set; }


        public bool IsCCandBCCVisible { get; set; }

        public string Topic { get; set; }

        public ICommand ShowCCandBCCFieldsCommand { get; set; }

        public AddressSectionViewModel()
        {
            ShowCCandBCCFieldsCommand = new RelayCommand(ShowCCandBCCFields);

            FromEmailsList = new ObservableCollection<MailWrapperViewModel>();
            ToEmailsList = new ObservableCollection<MailWrapperViewModel>();
            CCEmailsList = new ObservableCollection<MailWrapperViewModel>(); 
            BCCEmailsList = new ObservableCollection<MailWrapperViewModel>();

        }

        private void ShowCCandBCCFields() => IsCCandBCCVisible ^= true;
    }
}
