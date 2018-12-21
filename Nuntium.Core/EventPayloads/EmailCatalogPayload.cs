
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuntium.Core
{
    public class EmailCatalogPayload : EventArgs
    {
        public string EmailId;
        public int CatalogId;

        public EmailCatalogPayload(string EmailId, int CatalogId)
        {
            this.EmailId = EmailId;
            this.CatalogId = CatalogId;
        }
    }
}
