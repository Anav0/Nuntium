using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for InsertLinkPopup.xaml
    /// </summary>
    public partial class InsertLinkPopup : UserControl
    {
        public InsertLinkPopup()
        {
            InitializeComponent();
        }

        #region TextToDisplay

        public string TextToDisplay
        {
            get { return (string)GetValue(TextToDisplayProperty); }
            set { SetValue(TextToDisplayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextToDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextToDisplayProperty =
            DependencyProperty.Register("TextToDisplay", typeof(string), typeof(InsertLinkPopup), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region Link

        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Link.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register("Link", typeof(string), typeof(InsertLinkPopup), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region AcceptCommand

        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AcceptCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AcceptCommandProperty =
            DependencyProperty.Register("AcceptCommand", typeof(ICommand), typeof(InsertLinkPopup), new PropertyMetadata(null));

        #endregion

    }
}
