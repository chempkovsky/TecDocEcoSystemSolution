using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CarShop.Models
{
    class CarShopSalesContextInitializer : DropCreateDatabaseIfModelChanges<CarShopSalesContext>
    {
        protected override void Seed(CarShopSalesContext context)
        {

            context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchSpellHstTDES_OpenClose ON BranchSpellHstTDES(EntBranchGuid ASC, OpenAt DESC)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchSpellHstTDES_Close ON BranchSpellHstTDES(EntBranchGuid, CloseAt)");

            context.Database.ExecuteSqlCommand("CREATE INDEX IX_User2WorkPlaceTDES_EntBranchGuid ON User2WorkPlaceTDES(EntBranchGuid)");

            context.Database.ExecuteSqlCommand("CREATE INDEX IX_User2WorkPlaceHstTDES_WorkPlaceGuid ON User2WorkPlaceHstTDES(EntBranchGuid, WorkPlaceGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_User2WorkPlaceHstTDES_EntUserNic ON User2WorkPlaceHstTDES(EntBranchGuid, EntUserNic)");


            context.Database.ExecuteSqlCommand("CREATE INDEX IX_SaleBasketTDES_WorkPlaceGuid ON SaleBasketTDES(WorkPlaceGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_SaleBasketTDES_EntUserNic ON SaleBasketTDES(EntUserNic)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_SaleBasketTDES_SpellGuid ON SaleBasketTDES(SpellGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_SaleBasketTDES_EntBranchGuid ON SaleBasketTDES(EntBranchGuid)");


            context.Database.ExecuteSqlCommand("CREATE INDEX IX_ReturnBasketTDES_WorkPlaceGuid ON ReturnBasketTDES(WorkPlaceGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_ReturnBasketTDES_EntUserNic ON ReturnBasketTDES(EntUserNic)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_ReturnBasketTDES_SpellGuid ON ReturnBasketTDES(SpellGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_ReturnBasketTDES_EntBranchGuid ON ReturnBasketTDES(EntBranchGuid)");


            context.Database.ExecuteSqlCommand("CREATE INDEX IX_ReturnBasketArticleTDES_SpellGuid ON ReturnBasketArticleTDES(SpellGuid)");
            context.Database.ExecuteSqlCommand("CREATE INDEX IX_ReturnBasketArticleTDES_RetBasketGuid ON ReturnBasketArticleTDES(RetBasketGuid)");
            
        }
    }
}
