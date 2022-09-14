// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCategoryItemTecDocTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCategoryItemTecDocTDES
  {
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCategoryItemTDES_CategoryItemId", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public int CategoryItemId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCategoryTDES_CategoryId", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 1)]
    [Required]
    public int CategoryId { get; set; }

    public virtual EnterpriseCategoryTecDocTDES EnterpriseCategory { get; set; }

    [Required]
    [Display(Name = "EnterpriseCategoryItemTDES_EntCategoryItemDescriptionId", ResourceType = typeof (Resources))]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EntCategoryItemDescriptionId { get; set; }

    public virtual EnterpriseCategoryItemTecDocDescriptionTDES EnterpriseCategoryItemTecDocDescription { get; set; }
  }
}
