using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;



namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseTDES // TecDocEcoSystem
    {
        [Key]
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [RequiredAttribute]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntDescription")]
        public string EntDescription { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_ArticleCatalog")]
        public string ArticleCatalog { get; set; }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_TecDocSrcTypeId")]
        public int TecDocSrcTypeId { get; set; }
        public virtual EnterpriseTecDocSrcTypeTDES EnterpriseTecDocSrcTypeTDES { get; set; }


        public virtual ICollection<EnterpriseUserTDES> EnterpriseUserTDESes { get; set; }
        public virtual ICollection<EnterpriseBranchTDES> EnterpriseBranchTDESes { get; set; }
        public virtual ICollection<EnterpriseAddressTDES> EnterpriseAddressTDESes { get; set; }
        public virtual ICollection<EnterpriseSupplierTDES> EnterpriseSupplierTDESes { get; set; }

    }
}

