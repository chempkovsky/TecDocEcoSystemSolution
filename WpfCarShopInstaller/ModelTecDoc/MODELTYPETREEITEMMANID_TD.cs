using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarShopDataService.Properties;
using System.ComponentModel.DataAnnotations;



namespace TecDocEcoSystemDbClassLibrary
{
    public class MODELTYPETREEITEMMANID_TD
    {
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_SUP_TEXT")]
        public string SUP_TEXT { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_VALUE")]
        public string TEX_VALUE { get; set; }
    }
}