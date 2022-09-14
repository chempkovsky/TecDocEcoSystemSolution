// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseBrandTDES
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
  public class EnterpriseBrandTDES
  {
    [Key]
    [Required]
    [Display(Name = "EnterpriseBrandTDES_EntBrandNic", ResourceType = typeof (Resources))]
    [StringLength(40)]
    [Column(Order = 0)]
    public string EntBrandNic { get; set; }

    [Display(Name = "EnterpriseBrandTDES_EntGuid", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Required]
    [Key]
    public Guid EntGuid { get; set; }

    [StringLength(80)]
    [Required]
    [Display(Name = "EnterpriseBrandTDES_EntBrandDescription", ResourceType = typeof (Resources))]
    public string EntBrandDescription { get; set; }

    public virtual ICollection<EnterpriseArticleTDES> EnterpriseArticleTDESes { get; set; }
  }
}
