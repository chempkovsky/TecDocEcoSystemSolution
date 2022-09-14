// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleTecDocTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleTecDocTDES
  {
    [Required]
    [Key]
    [Display(Name = "EnterpriseArticleCategoryItemTDES_ArticleId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ArticleId { get; set; }

    [Display(Name = "EnterpriseArticleTDES_ExternArticle", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Required]
    public string ExternArticle { get; set; }

    [StringLength(40)]
    [Display(Name = "EnterpriseArticleTDES_ExternBrandNic", ResourceType = typeof (Resources))]
    [Required]
    public string ExternBrandNic { get; set; }

    [StringLength(20)]
    [Display(Name = "EnterpriseArticleTDES_ExternArticleEAN", ResourceType = typeof (Resources))]
    public string ExternArticleEAN { get; set; }

    [Display(Name = "EnterpriseArticleBrandTDES_ArticleBrandId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public int ArticleBrandId { get; set; }

    [Required]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EntArticleDescriptionId { get; set; }

    public virtual EnterpriseArticleTecDocDescriptionTDES EnterpriseArticleTecDocDescription { get; set; }
  }
}
