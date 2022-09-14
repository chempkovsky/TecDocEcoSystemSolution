using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;

namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseBranchWorkPlaceTDES
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "WorkPlaceGuid")]
        public Guid WorkPlaceGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }


        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }


        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }
        public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }


        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

    }
}