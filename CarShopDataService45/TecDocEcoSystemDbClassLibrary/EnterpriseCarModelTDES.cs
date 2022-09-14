// Decompiled with JetBrains decompiler
// Type: TecDocEcoSystemDbClassLibrary.EnterpriseCarModelTDES
// Assembly: CarShopDataService45, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DC6E29D1-19C5-4FCA-B19D-2A53232AAA8E
// Assembly location: C:\Development\WebCarShop\CarShopDataService45.dll

using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
  public class EnterpriseCarModelTDES
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCarModelTypeTDES_EnterpriseCarBrandId", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 0)]
    [Required]
    public int EnterpriseCarBrandId { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCarModelTypeId", ResourceType = typeof (Resources))]
    [Key]
    [Column(Order = 1)]
    public int EnterpriseCarModelTypeId { get; set; }

    public virtual EnterpriseCarModelTypeTDES EnterpriseCarModelTypeTDES { get; set; }

    [Column(Order = 2)]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "EnterpriseCarModelId", ResourceType = typeof (Resources))]
    [Key]
    public int EnterpriseCarModelId { get; set; }

    [Required]
    [StringLength(80)]
    [Display(Name = "TECDOC_TEX_TEXT", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelName { get; set; }

    [Display(Name = "TECDOC_TYP_KV_BODY", ResourceType = typeof (Resources))]
    [Required]
    [StringLength(80)]
    public string EnterpriseCarModelBody { get; set; }

    [StringLength(10)]
    [Display(Name = "TECDOC_TYP_PCON_START", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelProductDateStart { get; set; }

    [StringLength(10)]
    [Display(Name = "TECDOC_TYP_PCON_END", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelProductDateTil { get; set; }

    [StringLength(10)]
    [Display(Name = "TECDOC_TYP_KW_FROM", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelPowerKW { get; set; }

    [StringLength(10)]
    [Display(Name = "TECDOC_TYP_HP_FROM", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelPowerHP { get; set; }

    [StringLength(10)]
    [Display(Name = "TECDOC_TYP_CCM", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelEngCap { get; set; }

    [StringLength(10)]
    [Display(Name = "TECDOC_TYP_VALVES", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelVALVES { get; set; }

    [Display(Name = "TECDOC_TYP_CYLINDERS", ResourceType = typeof (Resources))]
    [StringLength(10)]
    public string EnterpriseCarModelCYLINDERS { get; set; }

    [Display(Name = "TECDOC_TYP_KV_ABS", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EnterpriseCarModelABS { get; set; }

    [Display(Name = "TECDOC_TYP_KV_ASR", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EnterpriseCarModelASR { get; set; }

    [Display(Name = "TECDOC_TYP_KV_BRAKE_TYPE", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EnterpriseCarModelBrakeType { get; set; }

    [StringLength(80)]
    [Display(Name = "TECDOC_TYP_KV_BRAKE_SYST", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelBrakeSys { get; set; }

    [Required]
    [Display(Name = "TECDOC_TYP_KV_FUEL", ResourceType = typeof (Resources))]
    public int FUELId { get; set; }

    public virtual EnterpriseCarModelFuelTDES EnterpriseCarModelFuelTDES { get; set; }

    [StringLength(80)]
    [Display(Name = "TECDOC_TYP_KV_FUEL_SUPPLY", ResourceType = typeof (Resources))]
    public string EnterpriseCarModelFUELSUPPLY { get; set; }

    [Display(Name = "TECDOC_TYP_KV_CATALYST", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EnterpriseCarModelCATALYST { get; set; }

    [Display(Name = "TECDOC_TYP_KV_TRANS", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EnterpriseCarModelTRANS { get; set; }

    [Display(Name = "TECDOC_TYP_KV_ENGINE", ResourceType = typeof (Resources))]
    [StringLength(80)]
    public string EnterpriseCarModelENGCODE { get; set; }
  }
}
