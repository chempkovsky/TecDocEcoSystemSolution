using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarShop.Models
{
    public class CarShopRestContextInitializer : DropCreateDatabaseIfModelChanges<CarShopRestContext>
    {
        protected override void Seed(CarShopRestContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchRestTDES_ExternArticleEAN ON BranchRestTDES (ExternArticleEAN)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchRestTDES_TECDOC ON BranchRestTDES (ART_ARTICLE_NR, SUP_TEXT, EntBranchGuid)");

            context.Database.ExecuteSqlCommand("ALTER TABLE BranchRestTDES drop column IsReplicated");
            context.Database.ExecuteSqlCommand("ALTER TABLE BranchRestTDES ADD IsReplicated AS  DATEDIFF( ms, LastReplicated, LastUpdated)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchRestTDES_ISREPLY ON BranchRestTDES(EntBranchGuid, IsReplicated)");
        }
    }
}