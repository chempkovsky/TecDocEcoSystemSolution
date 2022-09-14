using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;

namespace TecDocEcoSystemDbClassLibrary
{
    public class MODELTYPE_TD
    {
        [Key]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_ID")]
        public int TYP_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string TEX_TEXT { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_BODY")]
        public string TYP_KV_BODY { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_PCON_START")]
        public string TYP_PCON_START { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_PCON_END")]
        public string TYP_PCON_END { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KW_FROM")]
        public string TYP_KW_FROM { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KW_UPTO")]
        public string TYP_KW_UPTO { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_HP_FROM")]
        public string TYP_HP_FROM { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_HP_UPTO")]
        public string TYP_HP_UPTO { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_CCM")]
        public string TYP_CCM { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_VALVES")]
        public string TYP_VALVES { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_CYLINDERS")]
        public string TYP_CYLINDERS { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_DOORS")]
        public string TYP_DOORS { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_ABS")]
        public string TYP_KV_ABS { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_ASR")]
        public string TYP_KV_ASR { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_BRAKE_TYPE")]
        public string TYP_KV_BRAKE_TYPE { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_BRAKE_SYST")]
        public string TYP_KV_BRAKE_SYST { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_FUEL")]
        public string TYP_KV_FUEL { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_FUEL_SUPPLY")]
        public string TYP_KV_FUEL_SUPPLY { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_CATALYST")]
        public string TYP_KV_CATALYST { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_TRANS")]
        public string TYP_KV_TRANS { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_ENGINE")]
        public string TYP_KV_ENGINE { get; set; }

        public int FUEL_ID { get; set; }

    }


}