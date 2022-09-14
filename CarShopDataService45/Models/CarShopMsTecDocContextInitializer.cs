// Decompiled with JetBrains decompiler
// Type: CarShop.Models.CarShopMsTecDocContextInitializer
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using System.Data.Entity;

namespace CarShop.Models
{
  public class CarShopMsTecDocContextInitializer : DropCreateDatabaseIfModelChanges<CarShopMsTecDocContext>
  {
    protected override void Seed(CarShopMsTecDocContext context)
    {
      context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_EnterpriseArticleTecDocDescriptionTDES_Name ON EnterpriseArticleTecDocDescriptionTDES (EntArticleDescription)");
      context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_EnterpriseCategoryItemTecDocDescriptionTDES_Name ON EnterpriseCategoryItemTecDocDescriptionTDES (EntCategoryItemDescription)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseCategoryTecDocTDES_Parent ON EnterpriseCategoryTecDocTDES(CategoryParent)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleLookUpTDES ON EnterpriseArticleLookUpTDES(ArticleId, ArticleBrandId)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleApplicTDES ON EnterpriseArticleApplicTDES(EnterpriseCarModelId)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleBrandTDES ON EnterpriseArticleBrandTDES(ArticleBrandNic)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleCategoryItemTDES ON EnterpriseArticleCategoryItemTDES(CategoryItemId)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleTecDocTDES_Extern ON EnterpriseArticleTecDocTDES(ExternArticle, ExternBrandNic)");
      context.Database.ExecuteSqlCommand("CREATE INDEX IX_EnterpriseArticleTecDocTDES_EAN ON EnterpriseArticleTecDocTDES(ExternArticleEAN)");
    }
  }
}
