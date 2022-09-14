// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleApplicTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleApplicTDES
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseArticleCategoryItemTDES_ArticleId", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    [Required]
    public int ArticleId { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCarModelId", ResourceType = typeof (Resources))]
    [Required]
    public int EnterpriseCarModelId { get; set; }
  }
}
