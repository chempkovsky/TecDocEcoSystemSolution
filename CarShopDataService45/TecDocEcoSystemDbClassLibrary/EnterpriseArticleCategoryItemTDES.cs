// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleCategoryItemTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleCategoryItemTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 0)]
    [Key]
    [Display(Name = "EnterpriseArticleCategoryItemTDES_ArticleId", ResourceType = typeof (Resources))]
    [Required]
    public int ArticleId { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 1)]
    [Required]
    [Display(Name = "EnterpriseCategoryItemTDES_CategoryItemId", ResourceType = typeof (Resources))]
    public int CategoryItemId { get; set; }
  }
}
