// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.Soato
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class Soato
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(10, MinimumLength = 10)]
    [Display(Name = "SoatoId", ResourceType = typeof (Resources))]
    [Required]
    [Key]
    public string SoatoId { get; set; }

    [Display(Name = "SoatoSettlementName", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(50)]
    public string SoatoSettlementName { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Range(1, 2147483647)]
    [Display(Name = "SoatoSettlementType", ResourceType = typeof (Resources))]
    public int SettlementTypeId { get; set; }

    [Display(Name = "SettlementTypeDescription", ResourceType = typeof (Resources))]
    public virtual SettlementType SettlementType { get; set; }
  }
}
