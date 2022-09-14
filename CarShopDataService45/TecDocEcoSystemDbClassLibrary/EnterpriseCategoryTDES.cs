// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryTDES
  {
    [Required]
    [Key]
    [Column(Order = 0)]
    [Display(Name = "EnterpriseCategoryTDES_CategoryId", ResourceType = typeof (Resources))]
    public int CategoryId { get; set; }

    [Display(Name = "EnterpriseCategoryTDES_EntGuid", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Required]
    [Key]
    public Guid EntGuid { get; set; }

    [StringLength(120)]
    [Required]
    [Display(Name = "EnterpriseCategoryTDES_CategoryDescription", ResourceType = typeof (Resources))]
    public string CategoryDescription { get; set; }

    [Required]
    [Display(Name = "EnterpriseCategoryTDES_CategoryParent", ResourceType = typeof (Resources))]
    public int CategoryParent { get; set; }

    public virtual ICollection<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESes { get; set; }
  }
}
