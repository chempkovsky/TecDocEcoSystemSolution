// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.AddressType
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class AddressType
  {
    [Key]
    [Required]
    [Display(Name = "AddressTypeId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Range(1, 2147483647)]
    public int AddressTypeId { get; set; }

    [StringLength(60)]
    [Display(Name = "AddressTypeDescription", ResourceType = typeof (Resources))]
    [Required]
    public string AddressTypeDescription { get; set; }
  }
}
