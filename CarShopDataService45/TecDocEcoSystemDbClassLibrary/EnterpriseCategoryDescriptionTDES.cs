// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryDescriptionTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryDescriptionTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "EnterpriseCategoryDescriptionTDES_EntCategoryDescriptionId", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public int EntCategoryDescriptionId { get; set; }

    [Display(Name = "EnterpriseCategoryDescriptionTDES_EntCategoryDescription", ResourceType = typeof (Resources))]
    [StringLength(120)]
    [Required]
    public string EntCategoryDescription { get; set; }

    public virtual ICollection<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESes { get; set; }
  }
}
