// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.StreetType
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class StreetType
  {
    [Key]
    [Display(Name = "StreetTypeId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Range(1, 2147483647)]
    public int StreetTypeId { get; set; }

    [Required]
    [StringLength(60)]
    [Display(Name = "StreetTypeDescription", ResourceType = typeof (Resources))]
    public string StreetTypeDescription { get; set; }
  }
}
