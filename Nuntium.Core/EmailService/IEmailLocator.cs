using System.Collections.Generic;

namespace Nuntium.Core
{
    public interface IEmailLocator
    {
        Email GetEmailById(string id);
        List<Email> GetAllEmails();
    }
}
