// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.Currency
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class Currency
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    [Display(Name = "CurrencyIso", ResourceType = typeof (Resources))]
    [Range(1, 2147483647)]
    public int CurrencyIso { get; set; }

    [Display(Name = "CurrencyCode3", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string CurrencyCode3 { get; set; }

    [Display(Name = "CurrencyName", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(50)]
    public string CurrencyName { get; set; }

    [Display(Name = "FractionalUnit", ResourceType = typeof (Resources))]
    [Required]
    [Range(0, 2147483647)]
    public int FractionalUnit { get; set; }

    [StringLength(50)]
    [Display(Name = "FractionalUnitName", ResourceType = typeof (Resources))]
    [Required]
    public string FractionalUnitName { get; set; }

    [Display(Name = "IssuerName", ResourceType = typeof (Resources))]
    [StringLength(50)]
    public string IssuerName { get; set; }

    [Display(Name = "IsNational", ResourceType = typeof (Resources))]
    [Required]
    public bool IsNational { get; set; }

    [Display(Name = "IsOperational", ResourceType = typeof (Resources))]
    [Required]
    public bool IsOperational { get; set; }
  }
}
