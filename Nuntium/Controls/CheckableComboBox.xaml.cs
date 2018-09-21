using System.Windows;
using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for RadioComboBox.xaml
    /// </summary>
    public partial class CheckableComboBox : ComboBox
    {
        public CheckableComboBox()
        {
            InitializeComponent();
        }

        #region IsChecked

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked),
                typeof(bool), typeof(CheckableComboBox),
                new UIPropertyMetadata(false));

        #endregion

        #region IsThreeState

        public bool IsThreeState
        {
            get => (bool)GetValue(IsThreeStateProperty);
            set => SetValue(IsThreeStateProperty, value);
        }

        public static readonly DependencyProperty IsThreeStateProperty =
            DependencyProperty.Register(nameof(IsThreeState),
                typeof(bool), typeof(CheckableComboBox),
                new UIPropertyMetadata(false));

        #endregion

    }
}
