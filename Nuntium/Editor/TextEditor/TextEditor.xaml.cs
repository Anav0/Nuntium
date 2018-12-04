using Nuntium.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for TextEditor.xaml
    /// </summary>
    public partial class TextEditor : BasePage<TextEditorViewModel>
    {

        public TextEditor(TextEditorViewModel viewmodel) : base(viewmodel)
        {
            InitializeComponent();

            //TODO: Binding texteditor instance
            IoC.Kernel.Rebind<CustomRichTextBox>().ToConstant(editor);

        }

        #region BarBackground

        public Brush BarBackground
        {
            get { return (Brush)GetValue(BarBackgroundProperty); }
            set { SetValue(BarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty BarBackgroundProperty =
            DependencyProperty.Register("BarBackground", typeof(Brush), typeof(TextEditor), new PropertyMetadata(new Control().FindResource("DefaultTabBackgroundColorBrush")));

        #endregion

        #region BarForeground

        public Brush BarForeground
        {
            get { return (Brush)GetValue(BarForegroundProperty); }
            set { SetValue(BarForegroundProperty, value); }
        }

        public static readonly DependencyProperty BarForegroundProperty =
            DependencyProperty.Register("BarForeground", typeof(Brush), typeof(TextEditor), new PropertyMetadata(new Control().FindResource("DefaultTabForegroundColorBrush")));

        #endregion

    }
}
