using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class MODELTYPETREEITEMDESCR_TD
    {
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string TEX_TEXT { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_VALUE")]
        public string TEX_VALUE { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_UNIT")]
        public string TEX_UNIT { get; set; }
    }
}