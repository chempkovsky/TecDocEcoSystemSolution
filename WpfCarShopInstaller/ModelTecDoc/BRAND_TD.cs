using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;

namespace TecDocEcoSystemDbClassLibrary
{
    public class BRAND_TD
    {
        [Key]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_MFA_ID")]
        public int MFA_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_MFA_BRAND")]
        public string MFA_BRAND { get; set; }
    }
}