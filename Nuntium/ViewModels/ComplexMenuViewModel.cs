using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nuntium
{
    public class ComplexMenuViewModel : BaseViewModel
    {
        public ObservableCollection<MoreOptionsItemViewModel> PrevItems { get; set; }

        public ObservableCollection<MoreOptionsItemViewModel> Items { get; set; }

        public ICommand GoBackCommand { get; set; }

        public ComplexMenuViewModel()
        {

            Items = new ObservableCollection<MoreOptionsItemViewModel>();

            PrevItems = new ObservableCollection<MoreOptionsItemViewModel>();
            GoBackCommand = new RelayCommand(() =>
            {
                Items = new ObservableCollection<MoreOptionsItemViewModel>(PrevItems);
                PrevItems.Clear();
            });
        }

        public void SwapContent(ObservableCollection<MoreOptionsItemViewModel> contentItems)
        {
            if (contentItems == Items) return;

            PrevItems = new ObservableCollection<MoreOptionsItemViewModel>(Items);

            Items = new ObservableCollection<MoreOptionsItemViewModel>(contentItems);
        }
            
    }
}
