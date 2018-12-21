using Ninject;
using Nuntium.Core;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows;

namespace Nuntium
{
    public class MessageMiniatureListViewModel : BaseViewModel
    {
        public ObservableCollection<MessageMiniatureViewModel> Items { get; set; } = new ObservableCollection<MessageMiniatureViewModel>();

        public ObservableCollection<MessageMiniatureViewModel> SelectedItems { get; set; } = new ObservableCollection<MessageMiniatureViewModel>();

        public MessageMiniatureViewModel Selected { get; set; }

        public MessageMiniatureListViewModel(IEventAggregator eventAggregator, ICatalogService catalogService)
        {
            eventAggregator?.GetEvent<MarkEmailAsUnread>().Subscribe((id) =>
            {
                Items.ForEach(email => { if (email.Id == id) email.WasRead = false; });
            });

            
        }

    }
}
