using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class RevaluationArticleTDES
    {
//--------------------------------------------
        [Key, Column(Order = 0)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticle")]
        public string EntArticle { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntBrandNic")]
        public string EntBrandNic { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "RevaluationArticleTDES_IncomePayRollTDES")]
        public Guid IncomePayRollTDESGuid { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public string EntArticleDescription { get; set; }

//--------------------------------------------
        [Key, Column(Order = 3)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "SheetRevaluationTDES_SheetRevaluationTDESGuid")]
        public Guid SheetRevaluationTDESGuid { get; set; }
        public virtual SheetRevaluationTDES SheetRevaluationTDES { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }
//--------------------------------------------
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsProcessed")]
        public bool IsProcessed { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsReversed")]
        public bool IsReversed { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_CurrArtPrice")]
        [Range(0, Double.MaxValue)]
        public Double CurrArtPrice { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "RevaluationArticleTDES_NewArtPrice")]
        [Range(0, Double.MaxValue)]
        public Double NewArtPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_ArtAmountRest")]
        public int ArtAmountRest { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "RevaluationArticleTDES_OperSum")]
        public Double OperSum { get; set; }

        [Required]
        public int ProcessedState { get; set; }

        [StringLength(150)]
        [Display(ResourceType = typeof(Resources), Name = "RevaluationArticleTDES_Comments")]
        public string Comments { get; set; }
    }
}