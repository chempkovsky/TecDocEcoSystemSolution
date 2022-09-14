// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTmp
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryItemTmp
  {
    [Display(Name = "EnterpriseCategoryTDES_CategoryId", ResourceType = typeof (Resources))]
    [Required]
    public int CategoryItemId { get; set; }

    [Required]
    [Display(Name = "EnterpriseCategoryTDES_CategoryId", ResourceType = typeof (Resources))]
    public int CategoryId { get; set; }

    [Display(Name = "EnterpriseCategoryTDES_EntGuid", ResourceType = typeof (Resources))]
    [Required]
    public Guid EntGuid { get; set; }

    [Display(Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescription", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(120)]
    public string EntCategoryItemDescription { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseCategoryItemTmp_EntCategoryDescription", ResourceType = typeof (Resources))]
    public string EntCategoryDescription { get; set; }
  }
}
