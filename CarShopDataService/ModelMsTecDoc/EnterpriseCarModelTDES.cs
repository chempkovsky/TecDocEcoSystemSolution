using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCarModelTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelTypeTDES_EnterpriseCarBrandId")]
        public int EnterpriseCarBrandId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelTypeId")]
        public int EnterpriseCarModelTypeId { get; set; }
        public virtual EnterpriseCarModelTypeTDES EnterpriseCarModelTypeTDES { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelId")]
        public int EnterpriseCarModelId { get; set; }


        // Наименование
        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string EnterpriseCarModelName { get; set; }

        // Кузов
        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_BODY")]
        public string EnterpriseCarModelBody { get; set; }

        // начало выпуска
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_PCON_START")]
        public string EnterpriseCarModelProductDateStart { get; set; }

        // Конец выпуска
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_PCON_END")]
        public string EnterpriseCarModelProductDateTil { get; set; }

        // мощность KW
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KW_FROM")]
        public string EnterpriseCarModelPowerKW { get; set; }

        // мощность ЛС
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_HP_FROM")]
        public string EnterpriseCarModelPowerHP { get; set; }

        // объем
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_CCM")]
        public string EnterpriseCarModelEngCap { get; set; }

        // Клапанов
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_VALVES")]
        public string EnterpriseCarModelVALVES { get; set; }
        
        // Цилиндров
        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_CYLINDERS")]
        public string EnterpriseCarModelCYLINDERS { get; set; }

        // ABS
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_ABS")]
        public string EnterpriseCarModelABS { get; set; }

        // ASR
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_ASR")]
        public string EnterpriseCarModelASR { get; set; }

        // Вид тормозов
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_BRAKE_TYPE")]
        public string EnterpriseCarModelBrakeType { get; set; }

        // Система тормозов
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_BRAKE_SYST")]
        public string EnterpriseCarModelBrakeSys { get; set; }

        // вид топлива
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_FUEL")]
        public int FUELId { get; set; }
        public virtual EnterpriseCarModelFuelTDES EnterpriseCarModelFuelTDES { get; set; }

        /* 18 TYP_KV_FUEL_SUPPLY впрыск */ 
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_FUEL_SUPPLY")]
        public string EnterpriseCarModelFUELSUPPLY { get; set; }

        /* 19 TYP_KV_CATALYST катализатор */ 
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_CATALYST")]
        public string EnterpriseCarModelCATALYST { get; set; }

        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_TRANS")]
        public string EnterpriseCarModelTRANS { get; set; }

        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_ENGINE")]
        public string EnterpriseCarModelENGCODE { get; set; }
    }
}

