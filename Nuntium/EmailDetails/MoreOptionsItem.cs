using Nuntium.Core;
using System.Windows.Input;

namespace Nuntium
{
    public class MoreOptionsItemViewModel : BaseViewModel
    {
        public IconType Icon { get; set; }
        public string Text { get; set; }
        public ICommand Command { get; set; }

    }
}
