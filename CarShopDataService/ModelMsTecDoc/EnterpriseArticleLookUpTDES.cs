using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseArticleLookUpTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [StringLength(105)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleLookUpTDES_ArticleSearch")]
        public string ArticleSearch { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleCategoryItemTDES_ArticleId")]
        public int ArticleId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleBrandTDES_ArticleBrandId")]
        public int ArticleBrandId { get; set; }

        [Key, Column(Order = 3)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleBrandTDES_ArticleSearchKind")]
        public int ArticleSearchKind { get; set; }


        [StringLength(105)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleLookUpTDES_ArticleDysplay")]
        public string ArticleDysplay { get; set; }

    }
}