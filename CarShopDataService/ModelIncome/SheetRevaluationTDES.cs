using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class SheetRevaluationTDES
    {
        [Key]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "SheetRevaluationTDES_SheetRevaluationTDESGuid")]
        public Guid SheetRevaluationTDESGuid { get; set; }

        [Required]
        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "IncomePayRollTDES_Description")]
        public string Description { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserTDES_EntUserNic")]
        public string EntUserNic { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomePayRollTDES_CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsProcessed")]
        public bool IsProcessed { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsReversed")]
        public bool IsReversed { get; set; }

        public virtual ICollection<RevaluationArticleTDES> RevaluationArticleTDES { get; set; }
        
    }
}