using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarShopDBOrderDataService
{
    public class CarShopOrdersContextInitializer : DropCreateDatabaseIfModelChanges<CarShopOrdersContext>
    {
        protected override void Seed(CarShopOrdersContext context)
        {
            context.Database.ExecuteSqlCommand("ALTER TABLE GuestOrderTDES drop column IsReplicated ");
            context.Database.ExecuteSqlCommand("ALTER TABLE GuestOrderTDES ADD IsReplicated AS  DATEDIFF( ms, LastReplicated, LastUpdated)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_GuestOrderTDES_ISREPLY ON GuestOrderTDES(GestUserNic, IsReplicated)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_GuestOrderTDES_EntBranchGuid ON GuestOrderTDES(EntBranchGuid, IsReplicated)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_GuestOrderTDES_IsDone ON GuestOrderTDES(EntBranchGuid, IsDone)");


            context.Database.ExecuteSqlCommand("ALTER TABLE GuestOrderArticleTDES drop column IsReplicated ");
            context.Database.ExecuteSqlCommand("ALTER TABLE GuestOrderArticleTDES ADD IsReplicated AS  DATEDIFF( ms, LastReplicated, LastUpdated)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_GuestOrderArticleTDES_ISREPLY ON GuestOrderArticleTDES(GuestOrderGuid, IsReplicated)");

        }
    }
}
