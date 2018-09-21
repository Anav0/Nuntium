using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuntium
{
    public class ContactsListViewModel : BaseViewModel
    {
        public ObservableCollection<ContactViewModel> FilteredItems { get; set; }

        public List<ContactViewModel> AllItems { get; set; }

    }
}
