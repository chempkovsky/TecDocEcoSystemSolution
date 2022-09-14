// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.ContactType
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class ContactType
  {
    [Key]
    [Range(1, 2147483647)]
    [Display(Name = "ContactTypeId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public int ContactTypeId { get; set; }

    [StringLength(40)]
    [Required]
    [Display(Name = "ContactTypeDescription", ResourceType = typeof (Resources))]
    public string ContactTypeDescription { get; set; }
  }
}
