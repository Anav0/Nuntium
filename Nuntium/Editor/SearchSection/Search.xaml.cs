using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
        }

        #region SearchCommand

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(Search), new PropertyMetadata(null));

        #endregion

        #region SearchCommandParameter

        public object SearchCommandParameter
        {
            get { return (object)GetValue(SearchCommandParameterProperty); }
            set { SetValue(SearchCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchCommandParameterProperty =
            DependencyProperty.Register("SearchCommandParameter", typeof(object), typeof(Search), new PropertyMetadata(null));

        #endregion

        #region SearchPhrase

        public string SearchPhrase
        {
            get { return (string)GetValue(SearchPhraseProperty); }
            set { SetValue(SearchPhraseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchPhrase.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchPhraseProperty =
            DependencyProperty.Register("SearchPhrase", typeof(string), typeof(Search), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region PlaceholderText

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(Search), new FrameworkPropertyMetadata("Search..", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion
    }
}
