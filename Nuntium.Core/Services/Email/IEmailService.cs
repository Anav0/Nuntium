using System.Collections.Generic;

namespace Nuntium.Core
{
    public interface IEmailService
    {
        Email GetEmailById(string id);

        List<Email> GetAllEmails();


    }
}
