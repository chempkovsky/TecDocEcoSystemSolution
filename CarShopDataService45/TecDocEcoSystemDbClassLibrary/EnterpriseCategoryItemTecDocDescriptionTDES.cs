// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTecDocDescriptionTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryItemTecDocDescriptionTDES
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescriptionId", ResourceType = typeof (Resources))]
    [Required]
    public int EntCategoryItemDescriptionId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescription", ResourceType = typeof (Resources))]
    public string EntCategoryItemDescription { get; set; }
  }
}
