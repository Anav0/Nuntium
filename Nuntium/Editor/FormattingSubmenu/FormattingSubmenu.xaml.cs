using System;
using System.Windows;
using System.Windows.Controls;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for FormattingSubmenu.xaml
    /// </summary>
    public partial class FormattingSubmenu : UserControl
    {

        public FormattingSubmenu()
        {
            InitializeComponent();
        }

        public Control ActionTarget
        {
            get { return (Control)GetValue(ActionTargetProperty); }
            set { SetValue(ActionTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActionTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionTargetProperty =
            DependencyProperty.Register("ActionTarget", typeof(Control), typeof(FormattingSubmenu), new PropertyMetadata(null));



    }
}
