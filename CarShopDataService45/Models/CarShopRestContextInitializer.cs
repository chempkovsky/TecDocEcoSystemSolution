// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopRestContextInitializer
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopRestContextInitializer : DropCreateDatabaseIfModelChanges<CarShopRestContext>
  {
    protected override void Seed(CarShopRestContext context)
    {
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchRestTDES_ExternArticleEAN ON BranchRestTDES (ExternArticleEAN)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_BranchRestTDES_TECDOC ON BranchRestTDES (ART_ARTICLE_NR, SUP_TEXT, EntBranchGuid)");
    }
  }
}
