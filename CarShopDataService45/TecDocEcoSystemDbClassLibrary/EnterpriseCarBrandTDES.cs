// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCarBrandTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCarBrandTDES
  {
    [Display(Name = "EnterpriseCarBrandId", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EnterpriseCarBrandId { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "EnterpriseCarBrandName", ResourceType = typeof (Resources))]
    public string EnterpriseCarBrandName { get; set; }

    public virtual ICollection<EnterpriseCarModelTypeTDES> EnterpriseCarModelTypeTDESes { get; set; }
  }
}
