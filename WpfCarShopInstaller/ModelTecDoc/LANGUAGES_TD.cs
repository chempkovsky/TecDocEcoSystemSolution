using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class LANGUAGES_TD
    {
        [Key]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_LNG_ID")]
        public int LNG_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string TEX_TEXT { get; set; }
    }
}