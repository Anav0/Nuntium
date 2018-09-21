namespace Nuntium
{
    public class UserPopupDesign : UserPopupViewModel
    {
        public static UserPopupDesign Instance => new UserPopupDesign();

        public UserPopupDesign()
        {
            Fullname = "Janusz Pawlacz";
            UserInitials = "JP";
            Position = "History Teacher";
            ImagePath = "/Images/faces/if_matureman1_628284.png";
        }
    }
}
