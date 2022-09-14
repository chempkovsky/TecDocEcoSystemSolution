// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.BranchType
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class BranchType
  {
    [Required]
    [Display(Name = "BranchTypeId", ResourceType = typeof (Resources))]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Range(1, 2147483647)]
    public int BranchTypeId { get; set; }

    [Required]
    [Display(Name = "BranchTypeDescription", ResourceType = typeof (Resources))]
    [StringLength(60)]
    public string BranchTypeDescription { get; set; }
  }
}
