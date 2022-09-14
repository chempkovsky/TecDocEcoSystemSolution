using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class IncomeArticleTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_SupArticle")]
        public string SupArticle { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_SupBrandNic")]
        public string SupBrandNic { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomePayRollTDES_IncomePayRollTDES")]
        public Guid IncomePayRollTDESGuid { get; set; }
        public virtual IncomePayRollTDES IncomePayRollTDES { get; set; }


        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticle")]
        public string EntArticle { get; set; }
        
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntBrandNic")]
        public string EntBrandNic { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public string EntArticleDescription { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsProcessed")]
        public bool IsProcessed { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsReversed")]
        public bool IsReversed { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_ArtAmount")]
        public int ArtAmount { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_ArtAmountRest")]
        public int ArtAmountRest { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_PurchasePrice")]
        [Range(0, Double.MaxValue)]
        public Double PurchasePrice { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_ArtPrice")]
        [Range(0, Double.MaxValue)]
        public Double ArtPrice { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_CurrArtPrice")]
        [Range(0, Double.MaxValue)]
        public Double CurrArtPrice { get; set; }
        

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IncomeArticleTDES_IsRevaluate")]
        public bool IsRevaluate { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [StringLength(150)]
        [Display(ResourceType = typeof(Resources), Name = "RevaluationArticleTDES_Comments")]
        public string Comments { get; set; }

        [Required]
        public int ProcessedState { get; set; }


    }
}