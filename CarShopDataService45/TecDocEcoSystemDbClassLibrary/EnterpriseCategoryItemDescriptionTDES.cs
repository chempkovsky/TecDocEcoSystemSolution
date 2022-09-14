// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemDescriptionTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryItemDescriptionTDES
  {
    [Required]
    [Display(Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescriptionId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int EntCategoryItemDescriptionId { get; set; }

    [Display(Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescription", ResourceType = typeof (Resources))]
    [StringLength(120)]
    [Required]
    public string EntCategoryItemDescription { get; set; }

    public virtual ICollection<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESes { get; set; }
  }
}
