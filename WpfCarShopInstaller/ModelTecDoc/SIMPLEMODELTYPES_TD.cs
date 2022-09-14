using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class SIMPLEMODELTYPES_TD
    {
        [Key]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_ID")]
        public int TYP_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string TEX_TEXT { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_FUEL")]
        public int TYP_KV_FUEL { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_MOD_ID")]
        public int TYP_MOD_ID { get; set; }
    }
}