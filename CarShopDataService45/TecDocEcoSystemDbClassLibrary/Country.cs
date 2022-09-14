// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.Country
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class Country
  {
    [Key]
    [Required]
    [Range(1, 2147483647)]
    [Display(Name = "CountryIso", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CountryIso { get; set; }

    [Display(Name = "CountryCode2", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string CountryCode2 { get; set; }

    [Display(Name = "CountryCode3", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode3 { get; set; }

    [StringLength(50)]
    [Required]
    [Display(Name = "CountryName", ResourceType = typeof (Resources))]
    public string CountryName { get; set; }

    [Display(Name = "CountryEngName", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(50)]
    public string CountryEngName { get; set; }

    [Display(Name = "CountryCapital", ResourceType = typeof (Resources))]
    [StringLength(50)]
    public string CountryCapital { get; set; }

    [StringLength(10)]
    [Display(Name = "CountryPhone", ResourceType = typeof (Resources))]
    public string CountryPhone { get; set; }
  }
}
