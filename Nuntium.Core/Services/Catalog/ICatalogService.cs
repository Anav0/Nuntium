using System;
using System.Collections.Generic;
using System.Text;

namespace Nuntium.Core
{
    public interface ICatalogService
    {
        List<Catalog> GetAllCatalogs();

        Catalog GetCatalogById(int catalogId);
    }
}
