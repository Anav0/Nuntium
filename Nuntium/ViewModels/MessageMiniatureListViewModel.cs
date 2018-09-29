using System.Collections.ObjectModel;

namespace Nuntium
{
    public class MessageMiniatureListViewModel : BaseViewModel
    {
        public ObservableCollection<MessageMiniatureViewModel> Items { get; set; } = new ObservableCollection<MessageMiniatureViewModel>();

        public ObservableCollection<MessageMiniatureViewModel> SelectedItems { get; set; } = new ObservableCollection<MessageMiniatureViewModel>();

        public MessageMiniatureViewModel Selected { get; set; }

    }
}
