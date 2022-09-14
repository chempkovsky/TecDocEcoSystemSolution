using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;



namespace TecDocEcoSystemDbClassLibrary
{
    public class ReturnBasketArticleTDES
    {
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
        [Display(ResourceType = typeof(Resources), Name = "SaleBasketTDES_EntBasketGuid")]
        public Guid EntBasketGuid { get; set; }
        [Key, Column(Order = 3)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_EntBasketGuid")]
        public Guid RetBasketGuid { get; set; }


        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntArticleDescription")]
        public int EntArticleDescriptionId { get; set; }
        public virtual SaleArticleDescriptionTDES SaleArticleDescriptionTDES { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleTDES_EntGuid")]
        public Guid EntGuid { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserTDES_EntUserNic")]
        public string EntUserNic { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "SaleBasketArticleTDES_WorkPlaceGuid")]
        public Guid WorkPlaceGuid { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_SpellGuid")]
        public Guid SpellGuid { get; set; }


        // оплачено
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsPaid")]
        public bool IsPaid { get; set; }
        // время оплаты
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "SaleBasketTDES_PaidAt")]
        public DateTime PaidAt { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "SaleBasketArticle_ArtAmount")]
        public int ArtAmount { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "SaleBasketArticleTDES_SalePrice")]
        public Double SalePrice { get; set; }




        // сторно-возврат
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReverseAmount")]
        public int ReverseAmount { get; set; }

        [Required]
        public bool IsSpellClosed { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "CribFromIncome")]
        public int CribFromIncome { get; set; }

    }
}