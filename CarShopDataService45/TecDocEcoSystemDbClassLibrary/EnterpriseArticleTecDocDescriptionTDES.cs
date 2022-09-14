// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseArticleTecDocDescriptionTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseArticleTecDocDescriptionTDES
  {
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    public int EntArticleDescriptionId { get; set; }

    [Display(Name = "EnterpriseArticleTDES_EntArticleDescription", ResourceType = typeof (Resources))]
    [StringLength(120)]
    [Required]
    public string EntArticleDescription { get; set; }
  }
}
