using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuntium
{
    public class ContactListDesign : ContactsListViewModel
    {
        public static ContactListDesign Instance => new ContactListDesign();

        public ContactListDesign()
        {

            AllItems = new List<ContactViewModel>
            {
                new ContactViewModel
                {
                    PersonName = "Jacek Tabisz",
                    Initials = "JT",
                    PersonPosition = "History teacher"
                },
                new ContactViewModel
                {
                    PersonName = "Paweł Wrona",
                    Initials = "PW",
                    PersonPosition = "Math teacher"
                },
                new ContactViewModel
                {
                    PersonName = "Tomasz Zabłocki",
                    Initials = "TJ",
                    PersonPosition = "English teacher"
                },
                new ContactViewModel
                {
                    PersonName = "Jacek Tabisz",
                    Initials = "JT",
                    PersonPosition = "History teacher"
                },
            };

            FilteredItems = new ObservableCollection<ContactViewModel>(AllItems);

        }
    }
}
