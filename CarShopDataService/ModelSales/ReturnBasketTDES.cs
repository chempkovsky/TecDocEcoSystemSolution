using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class ReturnBasketTDES
    {
        [Key]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_EntBasketGuid")]
        public Guid RetBasketGuid { get; set; }

        /// <summary> 
        /// for reference of User2WorkPlaceHstTDES
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "WorkPlaceGuid")]
        public Guid WorkPlaceGuid { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserTDES_EntUserNic")]
        public string EntUserNic { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "User2WorkPlaceTDES_SetAt")]
        public DateTime SetAt { get; set; }

        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_Description")]
        public string Description { get; set; }

        /// <summary>
        /// end for reference of User2WorkPlaceHstTDES
        /// </summary>

        ///
        ///     reference onto Spell
        ///
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "BranchSpellTDES_SpellGuid")]
        public Guid SpellGuid { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_CreatedAt")]
        public DateTime CreatedAt { get; set; }

        // активная
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        // оплачено
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsPaid")]
        public bool IsPaid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_PaidAt")]
        public DateTime PaidAt { get; set; }

        // сторно
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsReverse")]
        public bool IsReverse { get; set; }

        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "Comments")]
        public string Comments { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_ArtAmount")]
        public int ArtAmount { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "ReturnBasketTDES_PaymentSum")]
        public Double PaymentSum { get; set; }

        public virtual ICollection<ReturnBasketArticleTDES> ReturnBasketArticleTDES { get; set; }

    }
}