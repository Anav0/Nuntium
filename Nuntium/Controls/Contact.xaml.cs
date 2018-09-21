using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Nuntium.Core;

namespace Nuntium
{
    /// <summary>
    /// Interaction logic for Contact.xaml
    /// </summary>
    public partial class Contact : UserControl
    {
        public Contact()
        {
            InitializeComponent();
        }


        //#region HasNewMessages

        //public bool HasNewMessages
        //{
        //    get => (bool)GetValue(HasNewMessagesProperty);
        //    set => SetValue(HasNewMessagesProperty, value);
        //}

        //public static readonly DependencyProperty HasNewMessagesProperty =
        //    DependencyProperty.Register(nameof(HasNewMessages),
        //        typeof(bool), typeof(Contact),
        //        new UIPropertyMetadata(false));

        //#endregion

        //#region PersonName

        //public string PersonName
        //{
        //    get => (string)GetValue(PersonNameProperty);
        //    set => SetValue(PersonNameProperty, value);
        //}

        //public static readonly DependencyProperty PersonNameProperty =
        //    DependencyProperty.Register(nameof(PersonName),
        //        typeof(string), typeof(Contact),
        //        new UIPropertyMetadata("Unknown contact"));

        //#endregion

        //#region PersonPosition

        //public string PersonPosition
        //{
        //    get => (string)GetValue(PersonPositionProperty);
        //    set => SetValue(PersonPositionProperty, value);
        //}

        //public static readonly DependencyProperty PersonPositionProperty =
        //    DependencyProperty.Register(nameof(PersonPosition),
        //        typeof(string), typeof(Contact),
        //        new UIPropertyMetadata("Unknown position"));

        //#endregion

        //#region Initials

        //public string Initials
        //{
        //    get => (string)GetValue(InitialsProperty);
        //    set => SetValue(InitialsProperty, value);
        //}

        //public static readonly DependencyProperty InitialsProperty =
        //    DependencyProperty.Register(nameof(Initials),
        //        typeof(string), typeof(Contact),
        //        new UIPropertyMetadata("NN"));

        //#endregion

        //#region AvatarBackground

        //public Brush AvatarBackground
        //{
        //    get => (Brush)GetValue(AvatarBackgroundProperty);
        //    set => SetValue(AvatarBackgroundProperty, value);
        //}

        //public static readonly DependencyProperty AvatarBackgroundProperty =
        //    DependencyProperty.Register(nameof(AvatarBackground),
        //        typeof(Brush), typeof(Contact),
        //        new UIPropertyMetadata(new BrushConverter().ConvertFromString(StringHelper.GenerateRandomColor())));

        //#endregion
    }
}
