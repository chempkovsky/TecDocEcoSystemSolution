using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class BranchRestTmp
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_EntBranchArticle")]
        public string EntBranchArticle { get; set; }

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
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public int EntArticleDescriptionId { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public string EntArticleDescription { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_ExternArticleEAN")]
        public string ExternArticleEAN { get; set; }


        public void CopyFrom(BranchRestTDES src)
        {
            if (src == null) return;

            EntBranchGuid = src.EntBranchGuid;
            EntBranchArticle = src.EntBranchArticle;
            EntBranchSup = src.EntBranchSup;

            ART_ARTICLE_NR = src.ART_ARTICLE_NR;
            SUP_TEXT = src.SUP_TEXT;

            ArtAmount = src.ArtAmount;
            ArtPrice = src.ArtPrice;

            LastUpdated = src.LastUpdated;
            LastReplicated = src.LastReplicated;
            TSConcClmn = src.TSConcClmn;

            ExternArticleEAN = src.ExternArticleEAN;

            EntArticleDescriptionId = src.EntArticleDescriptionId;
            if (src.BranchRestArticleDescriptionTDES != null)
                EntArticleDescription = src.BranchRestArticleDescriptionTDES.EntArticleDescription;
        }
        public void CopyTo(BranchRestTDES dest, bool doCreateDescr)
        {
            if (dest == null) return;
            dest.EntBranchGuid = EntBranchGuid;
            dest.EntBranchArticle = EntBranchArticle;
            dest.EntBranchSup = EntBranchSup;

            dest.ART_ARTICLE_NR = ART_ARTICLE_NR;
            dest.SUP_TEXT = SUP_TEXT;

            dest.ArtAmount = ArtAmount;
            dest.ArtPrice = ArtPrice;

            dest.LastUpdated = LastUpdated;
            dest.LastReplicated = LastReplicated;
            dest.TSConcClmn = TSConcClmn;

            dest.ExternArticleEAN = ExternArticleEAN;

            dest.EntArticleDescriptionId = EntArticleDescriptionId;
            if (doCreateDescr)
            {
                if (dest.BranchRestArticleDescriptionTDES == null) dest.BranchRestArticleDescriptionTDES = new BranchRestArticleDescriptionTDES();
                dest.BranchRestArticleDescriptionTDES.EntArticleDescription = EntArticleDescription;
                dest.BranchRestArticleDescriptionTDES.EntArticleDescriptionId = EntArticleDescriptionId;
            }

        }

    }
}