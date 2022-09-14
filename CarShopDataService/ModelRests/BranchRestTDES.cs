using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class BranchRestTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_EntBranchArticle")]
        public string EntBranchArticle { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_EntBranchSup")]
        public string EntBranchSup { get; set; }


        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_ART_ARTICLE_NR")]
        public string ART_ARTICLE_NR { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_SUP_TEXT")]
        public string SUP_TEXT { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_ArtAmount")]
        public int ArtAmount { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_ArtPrice")]
        public Double ArtPrice { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_LastUpdated")]
        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_LastReplicated")]
        public DateTime LastReplicated { get; set; }

        [Timestamp]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_TSConcClmn")]
        public Byte[] TSConcClmn { get; set; }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public int EntArticleDescriptionId { get; set; }
        public virtual BranchRestArticleDescriptionTDES BranchRestArticleDescriptionTDES { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_ExternArticleEAN")]
        public string ExternArticleEAN { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_IsReplicated")]
        public int IsReplicated { get; private set; }

    }
}