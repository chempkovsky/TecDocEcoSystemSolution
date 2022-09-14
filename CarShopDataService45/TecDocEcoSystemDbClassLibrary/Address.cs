// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.Address
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public abstract class Address
  {
    [Display(Name = "AddressGuid", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public Guid AddressGuid { get; set; }

    [Display(Name = "AddressType", ResourceType = typeof (Resources))]
    [Required]
    [Range(1, 2147483647)]
    public int AddressTypeId { get; set; }

    public virtual AddressType AddressType { get; set; }

    [Range(1, 2147483647)]
    [Required]
    [Display(Name = "CountryName", ResourceType = typeof (Resources))]
    public int CountryIso { get; set; }

    public virtual Country Country { get; set; }

    [Display(Name = "AddressRegion", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string AddressRegion { get; set; }

    [StringLength(80)]
    [Display(Name = "AddressDistrict", ResourceType = typeof (Resources))]
    public string AddressDistrict { get; set; }

    [StringLength(80)]
    [Display(Name = "AddressRural", ResourceType = typeof (Resources))]
    public string AddressRural { get; set; }

    [Display(Name = "SettlementType", ResourceType = typeof (Resources))]
    [Required]
    [Range(1, 2147483647)]
    public int SettlementTypeId { get; set; }

    public virtual SettlementType SettlementType { get; set; }

    [StringLength(80)]
    [Required]
    [Display(Name = "AddressSettlementName", ResourceType = typeof (Resources))]
    public string AddressSettlementName { get; set; }

    [Display(Name = "SoatoId", ResourceType = typeof (Resources))]
    [StringLength(10)]
    public string SoatoId { get; set; }

    public virtual Soato Soato { get; set; }

    [Range(1, 2147483647)]
    [Display(Name = "StreetType", ResourceType = typeof (Resources))]
    [Required]
    public int StreetTypeId { get; set; }

    public virtual StreetType StreetType { get; set; }

    [Display(Name = "AddressStreetName", ResourceType = typeof (Resources))]
    [StringLength(80)]
    [Required]
    public string AddressStreetName { get; set; }

    [StringLength(15)]
    [Display(Name = "AddressHouse", ResourceType = typeof (Resources))]
    public string AddressHouse { get; set; }

    [Display(Name = "AddressBuilding", ResourceType = typeof (Resources))]
    [StringLength(15)]
    public string AddressBuilding { get; set; }

    [Display(Name = "AddressOffice", ResourceType = typeof (Resources))]
    [StringLength(15)]
    public string AddressOffice { get; set; }

    [StringLength(15)]
    [Display(Name = "AddressPostCode", ResourceType = typeof (Resources))]
    public string AddressPostCode { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    [Display(Name = "AddressValidFrom", ResourceType = typeof (Resources))]
    [DataType(DataType.Date)]
    public DateTime AddressValidFrom { get; set; }

    [Display(Name = "AddressValidTo", ResourceType = typeof (Resources))]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime AddressValidTo { get; set; }

    [Required]
    [Display(Name = "IsActive", ResourceType = typeof (Resources))]
    public bool IsActive { get; set; }

    [Required]
    [Display(Name = "IsVisible", ResourceType = typeof (Resources))]
    public bool IsVisible { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    [Required]
    [Display(Name = "AddressLongitude", ResourceType = typeof (Resources))]
    public double AddressLongitude { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    [Display(Name = "AddressLatitude", ResourceType = typeof (Resources))]
    [Required]
    public double AddressLatitude { get; set; }
  }
}
