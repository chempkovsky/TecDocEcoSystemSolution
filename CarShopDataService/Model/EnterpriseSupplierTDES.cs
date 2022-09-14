using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;

namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseSupplierTDES
    {
        [Key, Column(Order = 0)]
        [Required]
//        [StringLength(40)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(40)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseSupplierTDES_EntSupplierId")]
        public string EntSupplierId { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EntSupplierDescription")]
        public string EntSupplierDescription { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        public virtual EnterpriseTDES EnterpriseTDES { get; set; }

        public virtual ICollection<EnterpriseSupplierContactTDES> EnterpriseSupplierContactTDESes { get; set; }

        public virtual ICollection<EnterpriseSupplierAddressTDES> EnterpriseSupplierAddressTDESes { get; set; }

    }
}