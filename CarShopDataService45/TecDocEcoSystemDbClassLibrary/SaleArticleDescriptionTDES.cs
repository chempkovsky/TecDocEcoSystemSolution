// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SaleArticleDescriptionTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SaleArticleDescriptionTDES
  {
    [Required]
    [Key]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EntArticleDescriptionId { get; set; }

    [StringLength(120)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [Required]
    public string EntArticleDescription { get; set; }

    public virtual ICollection<TecDocEcoSystemDbClassLibrary.SaleBasketArticleTDES> SaleBasketArticleTDES { get; set; }
  }
}
