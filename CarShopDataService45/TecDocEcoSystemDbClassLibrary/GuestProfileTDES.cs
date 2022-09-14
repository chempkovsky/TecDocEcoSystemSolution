// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.GuestProfileTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;

namespace TecDocEcoSystemDbClassLibrary
{
  public class GuestProfileTDES
  {
    [StringLength(80)]
    [Display(Name = "GuestProfileTDES_GestUserNic", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public string GestUserNic { get; set; }

    [Display(Name = "FirstName", ResourceType = typeof (Resources))]
    [StringLength(20)]
    public string FirstName { get; set; }

    [StringLength(20)]
    [Display(Name = "LastName", ResourceType = typeof (Resources))]
    public string LastName { get; set; }

    [StringLength(20)]
    [Display(Name = "MiddleName", ResourceType = typeof (Resources))]
    public string MiddleName { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "Phones", ResourceType = typeof (Resources))]
    public string Contact { get; set; }

    [StringLength(80)]
    [Display(Name = "ContactEmail", ResourceType = typeof (Resources))]
    public string ContactEmail { get; set; }

    [StringLength(160)]
    [Display(Name = "GuestProfileTDES_Address", ResourceType = typeof (Resources))]
    public string Address { get; set; }
  }
}
