using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for DefaultModal.xaml
    /// </summary>
    public partial class DefaultModal : UserControl
    {
        public DefaultModal()
        {
            InitializeComponent();
        }

        #region Header

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(DefaultModal), new PropertyMetadata());

        #endregion

        #region Message

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(DefaultModal), new PropertyMetadata(""));

        #endregion

        #region NoButtonText

        public string NoButtonText
        {
            get { return (string)GetValue(NoButtonTextProperty); }
            set { SetValue(NoButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoButtonTextProperty =
            DependencyProperty.Register("NoButtonText", typeof(string), typeof(DefaultModal), new PropertyMetadata("Cancel"));

        #endregion

        #region YesButtonText

        public string YesButtonText
        {
            get { return (string)GetValue(YesButtonTextProperty); }
            set { SetValue(YesButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YesButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YesButtonTextProperty =
            DependencyProperty.Register("YesButtonText", typeof(string), typeof(DefaultModal), new PropertyMetadata("Accept"));

        #endregion

        #region NoButtonBackground

        public Brush NoButtonBackground
        {
            get { return (Brush)GetValue(NoButtonBackgroundProperty); }
            set { SetValue(NoButtonBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoButtonBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoButtonBackgroundProperty =
            DependencyProperty.Register("NoButtonBackground", typeof(Brush), typeof(DefaultModal), new PropertyMetadata(new SolidColorBrush(Colors.PaleVioletRed)));

        #endregion

        #region YesButtonBackground

        public Brush YesButtonBackground
        {
            get { return (Brush)GetValue(YesButtonBackgroundProperty); }
            set { SetValue(YesButtonBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YesButtonBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YesButtonBackgroundProperty =
            DependencyProperty.Register("YesButtonBackground", typeof(Brush), typeof(DefaultModal), new PropertyMetadata(new SolidColorBrush(Colors.PaleGreen)));

        #endregion

        #region NoButtonForeground

        public Brush NoButtonForeground
        {
            get { return (Brush)GetValue(NoButtonForegroundProperty); }
            set { SetValue(NoButtonForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoButtonForground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoButtonForegroundProperty =
            DependencyProperty.Register("NoButtonForeground", typeof(Brush), typeof(DefaultModal), new PropertyMetadata(new SolidColorBrush( Colors.White)));

        #endregion

        #region YesButtonForeground

        public Brush YesButtonForeground
        {
            get { return (Brush)GetValue(YesButtonForegroundProperty); }
            set { SetValue(YesButtonForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YesButtonForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YesButtonForegroundProperty =
            DependencyProperty.Register("YesButtonForeground", typeof(Brush), typeof(DefaultModal), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region IsNoButtonVisible

        public bool IsNoButtonVisible
        {
            get { return (bool)GetValue(IsNoButtonVisibleProperty); }
            set { SetValue(IsNoButtonVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNoButtonVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNoButtonVisibleProperty =
            DependencyProperty.Register("IsNoButtonVisible", typeof(bool), typeof(DefaultModal), new PropertyMetadata(true));

        #endregion

        #region YesCommand

        public ICommand YesCommand
        {
            get { return (ICommand)GetValue(YesCommandProperty); }
            set { SetValue(YesCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YesCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YesCommandProperty =
            DependencyProperty.Register("YesCommand", typeof(ICommand), typeof(DefaultModal), new PropertyMetadata(null));

        #endregion

        #region NoCommand

        public ICommand NoCommand
        {
            get { return (ICommand)GetValue(NoCommandProperty); }
            set { SetValue(NoCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoCommandProperty =
            DependencyProperty.Register("NoCommand", typeof(ICommand), typeof(DefaultModal), new PropertyMetadata(null));

        #endregion
    }
}
