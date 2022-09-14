// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleBrandTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleBrandTDES
  {
    [Display(Name = "EnterpriseArticleBrandTDES_ArticleBrandId", ResourceType = typeof (Resources))]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public int ArticleBrandId { get; set; }

    [Display(Name = "EnterpriseArticleBrandTDES_ArticleBrandNic", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(40)]
    public string ArticleBrandNic { get; set; }
  }
}
