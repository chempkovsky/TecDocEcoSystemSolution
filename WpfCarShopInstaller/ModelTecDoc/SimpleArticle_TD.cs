using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class SimpleArticle_TD
    {
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_ART_ID")]
        public int ART_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_ART_ARTICLE_NR")]
        public string ART_ARTICLE_NR { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_SUP_TEXT")]
        public string SUP_TEXT { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_MASTER_BEZ")]
        public string MASTER_BEZ { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_GA_NR")]
        public int GA_NR { get; set; }

    }

    public class SimpleArticle_REST_TD : SimpleArticle_TD
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_ArtAmount")]
        public int ArtAmount { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchRestTDES_ArtPrice")]
        public Double ArtPrice { get; set; }

    }

}