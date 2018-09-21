using Nuntium.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class BasePopup : UserControl
    {
        public BasePopup()
        {
            InitializeComponent();
        }

        #region ArrowVisible

        public static readonly DependencyProperty ArrowVisibleProperty = DependencyProperty.Register(
        "ArrowVisible",
        typeof(ArrowDirection),
        typeof(BasePopup),
        new FrameworkPropertyMetadata(ArrowDirection.Bottom, new PropertyChangedCallback(ArrowVisiblePropertyChanged))
        );

        public static void SetArrowVisible(BasePopup element, ArrowDirection value)
        {
            element.SetValue(ArrowVisibleProperty, value);
        }

        public static ArrowDirection GetArrowVisible(BasePopup element)
        {
            return (ArrowDirection)element.GetValue(ArrowVisibleProperty);
        }

        private async static void ArrowVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is BasePopup element))
                return;

            if (e.OldValue == e.NewValue)
                return;

            await Task.Delay(5);
            element.TopArrow.Visibility = Visibility.Collapsed;
            element.BottomArrow.Visibility = Visibility.Collapsed;
            element.LeftArrow.Visibility = Visibility.Collapsed;
            element.RightArrow.Visibility = Visibility.Collapsed;

            switch ((ArrowDirection)e.NewValue)
            {
                case ArrowDirection.Top:
                    element.TopArrow.Visibility = Visibility.Visible;
                    break;
                case ArrowDirection.Bottom:
                    element.BottomArrow.Visibility = Visibility.Visible;
                    break;
                case ArrowDirection.Left:
                    element.LeftArrow.Visibility = Visibility.Visible;
                    break;
                case ArrowDirection.Right:
                    element.RightArrow.Visibility = Visibility.Visible;
                    break;
            }

            
        }

        #endregion

    }
}
