using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;



namespace TecDocEcoSystemDbClassLibrary
{
    public class BranchSpellTDES
    {

        [Key]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EntBranchDescription")]
        public string EntBranchDescription { get; set; }



        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_SpellGuid")]
        public Guid SpellGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_IsActive")]
        public bool IsActive  { get; set; }

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
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Timestamp]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_TSConcClmn")]
        public Byte[] TSConcClmn { get; set; }

        // we can not delete the slot if foreign key will be
//        public virtual ICollection<BranchSpellHstTDES> BranchSpellHstTDESes { get; set; }

    }
}