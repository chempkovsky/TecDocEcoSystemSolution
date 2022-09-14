// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCarModelTypeTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCarModelTypeTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCarModelTypeTDES_EnterpriseCarBrandId", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 0)]
    [Required]
    public int EnterpriseCarBrandId { get; set; }

    public virtual EnterpriseCarBrandTDES EnterpriseCarBrandTDES { get; set; }

    [Key]
    [Display(Name = "EnterpriseCarModelTypeId", ResourceType = typeof (Resources))]
    [Column(Order = 1)]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EnterpriseCarModelTypeId { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "EnterpriseCarModelTypeName", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelTypeName { get; set; }

    public virtual ICollection<EnterpriseCarModelTDES> EnterpriseCarModelTDESes { get; set; }
  }
}
