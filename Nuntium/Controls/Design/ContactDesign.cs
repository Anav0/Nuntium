namespace Nuntium
{
    public class ContactDesign : ContactViewModel
    {

        public static ContactDesign Instance => new ContactDesign();

        public ContactDesign()
        {

            PersonName = "Igor Motyka";
            PersonPosition = "App developer";
            Initials = "IM";
            HasNewMessages = true;
        }
    }
}
