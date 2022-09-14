using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
    using CarShopDataService.Properties;

    public class GuestOrderArticleTDES
    {

        [Key, Column(Order = 0)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderTDES_GuestOrderGuid")]
        public Guid GuestOrderGuid { get; set; }
        public virtual GuestOrderTDES GuestOrderTDES { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_EntBranchArticle")]
        public string EntBranchArticle { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_EntBranchSup")]
        public string EntBranchSup { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public string EntArticleDescription { get; set; }


        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_ART_ARTICLE_NR")]
        public string ART_ARTICLE_NR { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_SUP_TEXT")]
        public string SUP_TEXT { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_ExternArticleEAN")]
        public string ExternArticleEAN { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_ArtAmount")]
        public int ArtAmount { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_ArtPrice")]
        public Double ArtPrice { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }



        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_LastUpdated")]
        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_LastReplicated")]
        public DateTime LastReplicated { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderArticleTDES_IsReplicated")]
        public int IsReplicated { get; private set; }

    }
}