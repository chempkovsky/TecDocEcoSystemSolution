// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryItemTDES
  {
    [Display(Name = "EnterpriseCategoryItemTDES_CategoryItemId", ResourceType = typeof (Resources))]
    [Column(Order = 0)]
    [Required]
    [Key]
    public int CategoryItemId { get; set; }

    [Display(Name = "EnterpriseCategoryTDES_CategoryId", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    [Column(Order = 1)]
    public int CategoryId { get; set; }

    [Display(Name = "EnterpriseCategoryTDES_EntGuid", ResourceType = typeof (Resources))]
    [Column(Order = 2)]
    [Required]
    [Key]
    public Guid EntGuid { get; set; }

    [Required]
    public virtual EnterpriseCategoryTDES EnterpriseCategory { get; set; }

    [Required]
    [Display(Name = "EnterpriseCategoryItemTDES_EntCategoryItemDescriptionId", ResourceType = typeof (Resources))]
    public int EntCategoryItemDescriptionId { get; set; }

    public virtual EnterpriseCategoryItemDescriptionTDES EnterpriseCategoryItemDescription { get; set; }

    [Required]
    [Display(Name = "EnterpriseCategoryItemTDES_EntCategoryDescriptionId", ResourceType = typeof (Resources))]
    public int EntCategoryDescriptionId { get; set; }

    public virtual EnterpriseCategoryDescriptionTDES EnterpriseCategoryDescription { get; set; }
  }
}
