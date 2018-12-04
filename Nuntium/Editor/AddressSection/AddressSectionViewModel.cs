
using Nuntium.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Nuntium
{
    public class AddressSectionViewModel : BaseViewModel
    {

        public ObservableCollection<Address> Addresses { get; set; } = new ObservableCollection<Address>();

        public bool IsCCandBCCVisible { get; set; }

        public string Topic { get; set; }

        public ICommand ShowCCandBCCFieldsCommand { get; set; }

        public AddressSectionViewModel()
        {
            ShowCCandBCCFieldsCommand = new RelayCommand(ShowCCandBCCFields);
        }

        private void ShowCCandBCCFields() => IsCCandBCCVisible ^= true;
    }
}
