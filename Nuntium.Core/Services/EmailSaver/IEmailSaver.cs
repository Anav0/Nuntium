using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuntium.Core
{
    public interface IEmailSaver
    {
        void SaveEmailLocally(Email email);
    }
}
