using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CarShopDBDataServiceModule
{
    class CarShopContextInitializer : DropCreateDatabaseIfModelChanges<CarShopContext>
    {
        protected override void Seed(CarShopContext context)
        {
            // context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseCategoryTDES_Parent ON EnterpriseCategoryTDES(CategoryParent)");
        } 

    }
}
