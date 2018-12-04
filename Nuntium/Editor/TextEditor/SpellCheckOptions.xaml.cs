using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for SpellCheckOptions.xaml
    /// </summary>
    public partial class SpellCheckOptions : UserControl
    {
        public SpellCheckOptions()
        {
            InitializeComponent();
        }

        #region Items

        public ObservableCollection<string> Items
        {
            get { return (ObservableCollection<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<string>), typeof(SpellCheckOptions), new PropertyMetadata(new ObservableCollection<string>()));

        #endregion

        #region Selected

        public string Selected
        {
            get { return (string)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(string), typeof(SpellCheckOptions), new PropertyMetadata(null));

        #endregion

        #region CorrectionClicked

        public ICommand CorrectionClicked
        {
            get { return (ICommand)GetValue(CorrectionClickedProperty); }
            set { SetValue(CorrectionClickedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CorrectionClicked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CorrectionClickedProperty =
            DependencyProperty.Register("CorrectionClicked", typeof(ICommand), typeof(SpellCheckOptions), new PropertyMetadata(null));

        #endregion

        #region IgnoreCommand

        public ICommand IgnoreCommand
        {
            get { return (ICommand)GetValue(IgnoreCommandProperty); }
            set { SetValue(IgnoreCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreCommandProperty =
            DependencyProperty.Register("IgnoreCommand", typeof(ICommand), typeof(SpellCheckOptions), new PropertyMetadata(null));

        #endregion


    }
}
