// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryTecDocTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryTecDocTDES
  {
    [Key]
    [Display(Name = "EnterpriseCategoryTDES_CategoryId", ResourceType = typeof (Resources))]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(Order = 0)]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "EnterpriseCategoryTDES_CategoryDescription", ResourceType = typeof (Resources))]
    public string CategoryDescription { get; set; }

    [Required]
    [Display(Name = "EnterpriseCategoryTDES_CategoryParent", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CategoryParent { get; set; }

    public virtual ICollection<EnterpriseCategoryItemTecDocTDES> EnterpriseCategoryItemTecDocTDESes { get; set; }
  }
}
