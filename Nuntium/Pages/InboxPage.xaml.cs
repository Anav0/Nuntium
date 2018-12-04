using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for InboxPage.xaml
    /// </summary>
    public partial class InboxPage : UserControl
    {

        public InboxPage()
        {
            //TODO: this is just tmp solution
            DataContext = new InboxPageViewModel();

            InitializeComponent();
        }
    }
}
