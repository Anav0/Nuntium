using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for EmailDetailsAttachedSection.xaml
    /// </summary>
    public partial class EmailDetailsAttachedSection : UserControl
    {
        public EmailDetailsAttachedSection()
        {
            Items = new ObservableCollection<AttachFileViewModel>();

            InitializeComponent();
        }

        public ObservableCollection<AttachFileViewModel> Items
        {
            get { return (ObservableCollection<AttachFileViewModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<AttachFileViewModel>), typeof(EmailDetailsAttachedSection), new PropertyMetadata(null));


    }
}
