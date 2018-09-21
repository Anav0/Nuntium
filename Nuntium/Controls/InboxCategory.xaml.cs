using System.Windows;
using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for InboxCategory.xaml
    /// </summary>
    public partial class InboxCategory : RadioButton
    {
        public InboxCategory()
        {
            InitializeComponent();
        }

        #region NumberOfMessages

        public int NumberOfMessages
        {
            get => (int)GetValue(NumberOfMessagesProperty);
            set => SetValue(NumberOfMessagesProperty, value);
        }

        public static readonly DependencyProperty NumberOfMessagesProperty =
            DependencyProperty.Register(nameof(NumberOfMessages),
                typeof(int), typeof(InboxCategory),
                new UIPropertyMetadata(0));

        #endregion
    }
}
