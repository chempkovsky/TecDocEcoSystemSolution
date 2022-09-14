using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;

namespace TecDocEcoSystemDbClassLibrary
{
    public class BranchSpellHstTDES
    {

        [Key]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_SpellGuid")]
        public Guid SpellGuid { get; set; }
        

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsBlocked")]
        public bool IsBlocked { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_OpenAt")]
        public DateTime OpenAt { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_CloseAt")]
        public DateTime CloseAt { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_OpenedBy")]
        public string OpenedBy { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_ClosedBy")]
        public string ClosedBy { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }
        // we can not delete the slot if foreign key will be
//        public virtual BranchSpellTDES BranchSpellTDES { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

    }

}