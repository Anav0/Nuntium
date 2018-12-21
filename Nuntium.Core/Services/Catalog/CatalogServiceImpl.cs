using System;
using System.Collections.Generic;
using System.Text;

namespace Nuntium.Core
{
    public class CatalogServiceImpl : ICatalogService
    {
        private List<Catalog> mCatalogs;

        public CatalogServiceImpl()
        {
            mCatalogs = new List<Catalog>
            {
                new Catalog
                {
                    Id = 0,
                    Icon = IconType.Inbox,
                    DisplayName = "Inbox",
                    Category = InboxCategoryType.Inbox,
                },
                new Catalog
                {
                    Id = 1,
                    Icon = IconType.File,
                    DisplayName = "Drafts",
                    Category = InboxCategoryType.Drafts,
                },
                new Catalog
                {
                    Id = 2,
                    Icon = IconType.PaperPlane,
                    DisplayName = "Sent",
                    Category = InboxCategoryType.Sent,
                },
                new Catalog
                {
                    Id = 3,
                    Icon = IconType.Ban,
                    DisplayName = "Spam",
                    Category = InboxCategoryType.Spam,
                },
                new Catalog
                {
                    Id = 4,
                    Icon = IconType.Star,
                    DisplayName = "Stared",
                    Category = InboxCategoryType.Stared,
                },
                new Catalog
                {
                    Id = 5,
                    Icon = IconType.Archive,
                    DisplayName = "Archive",
                    Category = InboxCategoryType.Archive,
                },
                new Catalog
                {
                    Id = 6,
                    Icon = IconType.Bin,
                    DisplayName = "Deleted",
                    Category = InboxCategoryType.Deleted,
                }

            };
        }

        public List<Catalog> GetAllCatalogs() => mCatalogs;

        public Catalog GetCatalogById(int catalogId) => mCatalogs.Find(catalog => catalog.Id == catalogId);
    }
}
