using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseArticleTmp
    {
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticle")]
        public string EntArticle { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntBrandNic")]
        public string EntBrandNic { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntGuid")]
        public Guid EntGuid { get; set; }


        [Required]
        public int EntArticleDescriptionId { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public string EntArticleDescription { get; set; }



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


        public void CopyFrom(EnterpriseArticleTDES src)
        {
            if (src == null) return;

             EntArticle                =  src.EntArticle              ;
             EntBrandNic               =  src.EntBrandNic             ;
             EntGuid                   =  src.EntGuid                 ;
             EntArticleDescriptionId   =  src.EntArticleDescriptionId ;
             if (src.EnterpriseArticleDescriptionTDES != null) 
             EntArticleDescription     =  src.EnterpriseArticleDescriptionTDES.EntArticleDescription   ;
             ExternArticle             =  src.ExternArticle           ;
             ExternBrandNic            =  src.ExternBrandNic          ;
             ExternArticleEAN          =  src.ExternArticleEAN        ;
        }

        public void CopyTo(EnterpriseArticleTDES dest, bool doCreateDescr)
        {
            if (dest == null) return;

            dest.EntArticle = EntArticle;
            dest.EntBrandNic = EntBrandNic;
            dest.EntGuid = EntGuid;
            dest.EntArticleDescriptionId = EntArticleDescriptionId;
            if (doCreateDescr)
            {
                if (dest.EnterpriseArticleDescriptionTDES == null) dest.EnterpriseArticleDescriptionTDES = new EnterpriseArticleDescriptionTDES();
                dest.EnterpriseArticleDescriptionTDES.EntArticleDescription = EntArticleDescription;
                dest.EnterpriseArticleDescriptionTDES.EntArticleDescriptionId = EntArticleDescriptionId;
            }
            dest.ExternArticle = ExternArticle;
            dest.ExternBrandNic = ExternBrandNic;
            dest.ExternArticleEAN = ExternArticleEAN;
        }

    }
}