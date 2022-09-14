using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class FUEL_TD
    {
        [Key]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_DES_ID")]
        public int DES_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string TEX_TEXT { get; set; }
    }
}