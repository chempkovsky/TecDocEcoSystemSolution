// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseTecDocSrcTypeTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseTecDocSrcTypeTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseTecDocSrcTypeTDES_Id", ResourceType = typeof (Resources))]
    [Key]
    [Required]
    public int TecDocSrcTypeId { get; set; }

    [StringLength(60)]
    [Display(Name = "EnterpriseTecDocSrcTypeTDES_Descr", ResourceType = typeof (Resources))]
    [Required]
    public string TecDocSrcTypeDescr { get; set; }
  }
}
