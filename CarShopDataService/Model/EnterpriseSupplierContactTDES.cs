using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseSupplierContactTDES
    {
        [Key]
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserContactTDES_ContactGuid")]
        public Guid ContactGuid { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserContactTDES_Contact")]
        public string Contact { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsVisible")]
        public bool IsVisible { get; set; }

        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseSupplierTDES_EntSupplierId")]
        public string EntSupplierId { get; set; }
        public virtual EnterpriseSupplierTDES EnterpriseSupplierTDES { get; set; }

        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }
        // public virtual EnterpriseTDES EnterpriseTDES { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserContactTDES_ContactTypeId")]
        public int ContactTypeId { get; set; }
        public virtual ContactType ContactType { get; set; }


    }
}