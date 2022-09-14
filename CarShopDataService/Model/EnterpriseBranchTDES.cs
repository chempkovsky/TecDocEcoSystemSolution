using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;



namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseBranchTDES
    {
        [Key]
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EntBranchDescription")]
        public string EntBranchDescription { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsVisible")]
        public bool IsVisible { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "BranchType")]
        public int BranchTypeId { get; set; }
        public virtual BranchType BranchType { get; set; }


        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_TecDocCatalog")]
        public string TecDocCatalog { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_SalesCatalog")]
        public string SalesCatalog { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_IncomeCatalog")]
        public string IncomeCatalog { get; set; }


        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_OrderCatalog")]
        public string OrderCatalog { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }
        public virtual EnterpriseTDES EnterpriseTDES { get; set; }

        public virtual ICollection<EnterpriseBranchContactsTDES> EnterpriseBranchContactsTDESes { get; set; }

        public virtual ICollection<EnterpriseBranchUserTDES> EnterpriseBranchUserTDESes { get; set; }

        public virtual ICollection<EnterpriseBranchWorkPlaceTDES> EnterpriseBranchWorkPlaceTDESes { get; set; }

        public virtual ICollection<EnterpriseBranchAddressTDES> EnterpriseBranchAddressTDESes { get; set; }

        public virtual ICollection<EnterpriseBranchReplyTDES> EnterpriseBranchReplyTDES { get; set; }
    }
}