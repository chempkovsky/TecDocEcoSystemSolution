// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.SettlementType
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class SettlementType
  {
    [Range(1, 2147483647)]
    [Key]
    [Required]
    [Display(Name = "SettlementTypeId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int SettlementTypeId { get; set; }

    [StringLength(60)]
    [Required]
    [Display(Name = "SettlementTypeDescription", ResourceType = typeof (Resources))]
    public string SettlementTypeDescription { get; set; }

    [Display(Name = "SettlementTypeShortDescription", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(15)]
    public string SettlementTypeShortDescription { get; set; }
  }
}
