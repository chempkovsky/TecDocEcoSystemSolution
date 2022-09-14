using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseArticleApplicTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleCategoryItemTDES_ArticleId")]
        public int ArticleId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelId")]
        public int EnterpriseCarModelId { get; set; }

    }
}