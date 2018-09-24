using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nuntium
{
    public class MessageMiniatureListViewModel : BaseViewModel
    {
        public ObservableCollection<MessageMiniatureViewModel> Items { get; set; } = new ObservableCollection<MessageMiniatureViewModel>();

        public ObservableCollection<MessageMiniatureViewModel> SelectedItems { get; set; } = new ObservableCollection<MessageMiniatureViewModel>();

        private MessageMiniatureViewModel _Selected;
        public MessageMiniatureViewModel Selected
        {
            get => _Selected;

            set
            {
                if (_Selected == value)
                    return;

                _Selected = value;
                RaiseSelectedItemChangedEvent();
            }
        }

        public event EventHandler SelectedItemChanged;

        protected virtual void RaiseSelectedItemChangedEvent()
        {
            SelectedItemChanged?.Invoke(this, new EventArgs());
        }

    }
}
