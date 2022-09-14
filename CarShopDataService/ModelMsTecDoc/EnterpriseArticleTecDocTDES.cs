using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseArticleTecDocTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleCategoryItemTDES_ArticleId")]
        public int ArticleId { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_ExternArticle")]
        public string ExternArticle { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_ExternBrandNic")]
        public string ExternBrandNic { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_ExternArticleEAN")]
        public string ExternArticleEAN { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleBrandTDES_ArticleBrandId")]
        public int ArticleBrandId { get; set; }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public int EntArticleDescriptionId { get; set; }
        public virtual EnterpriseArticleTecDocDescriptionTDES EnterpriseArticleTecDocDescription { get; set; }

    }
}