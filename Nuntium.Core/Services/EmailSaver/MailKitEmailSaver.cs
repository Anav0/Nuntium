using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nuntium.Core
{
    public class MailKitEmailSaver : IEmailSaver
    {
        public void SaveEmailLocally(Email email)
        {
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress(email.Address));
            email.ToAddresses.ForEach(address => msg.To.Add(new MailboxAddress(address)));
            msg.Subject = email.Subject;

            //TODO: finish creating this email saver;
           
        }
    }
}
