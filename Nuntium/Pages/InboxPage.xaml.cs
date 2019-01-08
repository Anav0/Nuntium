using Ninject;
using Nuntium.Core;
using Prism.Events;
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
            DataContext = new InboxPageViewModel(
                IoC.Kernel.Get<IEmailService>(), 
                IoC.Kernel.Get<IEventAggregator>(), 
                IoC.Kernel.Get<ICatalogService>(),
                IoC.Kernel.Get<CustomRichTextBox>(),
                IoC.Kernel.Get<AddressSectionViewModel>()
                );

            InitializeComponent();
        }
    }
}
