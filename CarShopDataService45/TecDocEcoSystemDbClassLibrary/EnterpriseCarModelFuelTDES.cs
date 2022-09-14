// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCarModelFuelTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCarModelFuelTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Display(Name = "TECDOC_TYP_KV_FUEL", ResourceType = typeof (Resources))]
    [Required]
    public int FUELId { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "FuelName", ResourceType = typeof (Resources))]
    public string FuelName { get; set; }

    public virtual ICollection<EnterpriseCarModelTDES> EnterpriseCarModelTDESes { get; set; }
  }
}
