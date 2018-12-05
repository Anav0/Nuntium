using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for RoundAvatar.xaml
    /// </summary>
    public partial class RoundAvatar : UserControl
    {
        public RoundAvatar()
        {
            InitializeComponent();
        }

        #region Initials

        public string Initials
        {
            get => (string)GetValue(InitialsProperty);
            set => SetValue(InitialsProperty, value);
        }

        public static readonly DependencyProperty InitialsProperty =
            DependencyProperty.Register(nameof(Initials),
                typeof(string), typeof(RoundAvatar),
                new UIPropertyMetadata());

        #endregion

        #region AvatarBackground

        public Brush AvatarBackground
        {
            get => (Brush)GetValue(AvatarBackgroundProperty);
            set => SetValue(AvatarBackgroundProperty, value);
        }

        public static readonly DependencyProperty AvatarBackgroundProperty =
            DependencyProperty.Register(nameof(AvatarBackground),
                typeof(Brush), typeof(RoundAvatar),
                new UIPropertyMetadata());

        #endregion

        #region ImagePath

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        public static readonly DependencyProperty ImagePathProperty=
            DependencyProperty.Register(nameof(ImagePath),
                typeof(string), typeof(RoundAvatar),
                new UIPropertyMetadata());

        #endregion
    }
}
