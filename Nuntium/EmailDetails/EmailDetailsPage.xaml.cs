using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for EmailDetails.xaml
    /// </summary>
    public partial class EmailDetailsPage : BasePage<EmailDetailsPageViewModel>
    {
        public EmailDetailsPage(EmailDetailsPageViewModel viewmodel) : base(viewmodel)
        {
            InitializeComponent();
        }
    }
}
