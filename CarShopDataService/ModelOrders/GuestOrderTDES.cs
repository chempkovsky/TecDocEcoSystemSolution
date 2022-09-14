using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
    using CarShopDataService.Properties;
    public class GuestOrderTDES
    {

        [Key]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderTDES_GuestOrderGuid")]
        public Guid GuestOrderGuid { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "GuestProfileTDES_GestUserNic")]
        public string GestUserNic { get; set; }
        public virtual GuestProfileTDES GuestProfileTDES { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EntBranchDescription")]
        public string EntBranchDescription { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderTDES_IsDone")]
        public bool IsDone { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderTDES_LastUpdated")]
        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderTDES_LastReplicated")]
        public DateTime LastReplicated { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(ResourceType = typeof(Resources), Name = "GuestOrderTDES_IsReplicated")]
        public int IsReplicated { get; private set; }

        public virtual ICollection<GuestOrderArticleTDES> GuestOrderArticleTDESes { get; set; }

    }
}