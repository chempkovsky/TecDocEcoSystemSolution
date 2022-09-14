// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopArticleContextInitializer
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopArticleContextInitializer : DropCreateDatabaseIfModelChanges<CarShopArticleContext>
  {
    protected override void Seed(CarShopArticleContext context)
    {
      context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_EnterpriseArticleDescriptionTDES_Name ON EnterpriseArticleDescriptionTDES (EntArticleDescription)");
      context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_EnterpriseCategoryDescriptionTDES_Name ON EnterpriseCategoryDescriptionTDES (EntCategoryDescription)");
      context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_EnterpriseCategoryItemDescriptionTDES_Name ON EnterpriseCategoryItemDescriptionTDES (EntCategoryItemDescription)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseCategoryTDES_Parent ON EnterpriseCategoryTDES(CategoryParent)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleTDES_Extern ON EnterpriseArticleTDES(ExternArticle, ExternBrandNic)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleTDES_EAN ON EnterpriseArticleTDES(ExternArticleEAN)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleTDES_EntGuid ON EnterpriseArticleTDES(EntGuid)");
    }
  }
}
