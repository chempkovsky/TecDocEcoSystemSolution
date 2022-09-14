using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CarShop.Models
{
    public class CarShopIncomeContextInitializer : DropCreateDatabaseIfModelChanges<CarShopIncomeContext>
    {
        protected override void Seed(CarShopIncomeContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_IncomePayRollTDES ON IncomePayRollTDES (EntGuid, EntBranchGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_IncomeArticleTDES ON IncomeArticleTDES (EntArticle, EntBrandNic, EntGuid, EntBranchGuid)");
        }
    }
}
