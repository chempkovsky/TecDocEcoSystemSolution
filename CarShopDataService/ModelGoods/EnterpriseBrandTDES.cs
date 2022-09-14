using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseBrandTDES
    {

        [Key, Column(Order = 0)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBrandTDES_EntBrandNic")]
        public string EntBrandNic { get; set; }

        
        [Key, Column(Order = 1)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBrandTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        
        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBrandTDES_EntBrandDescription")]
        public string EntBrandDescription { get; set; }

        public virtual ICollection<EnterpriseArticleTDES> EnterpriseArticleTDESes { get; set; }
    }
}