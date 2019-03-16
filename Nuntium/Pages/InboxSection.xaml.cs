using Ninject;
using Nuntium.Core;
using Prism.Events;
using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for InboxSection.xaml
    /// </summary>
    public partial class InboxSection : UserControl
    {

        public InboxSection()
        {
            //TODO: Inject VM in different way
            DataContext = new InboxSectionViewModel(
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
