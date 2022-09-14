using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseSupplierAddressTDES : Address
    {
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseSupplierTDES_EntSupplierId")]
        public string EntSupplierId { get; set; }
        public virtual EnterpriseSupplierTDES EnterpriseSupplierTDES { get; set; }
    }
}