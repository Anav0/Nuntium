
namespace Nuntium
{
    public class MailWrapperDesign : MailWrapperViewModel
    {
        public static MailWrapperDesign Instance => new MailWrapperDesign();

        public MailWrapperDesign()
        {
            Address = "address@mail.com";
            FirstLetter = "A";
        }
    }
}
