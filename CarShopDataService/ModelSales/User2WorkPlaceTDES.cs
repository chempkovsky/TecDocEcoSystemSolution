using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class User2WorkPlaceTDES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "WorkPlaceGuid")]
        public Guid WorkPlaceGuid { get; set; }

        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }

        
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserTDES_EntUserNic")]
        public string EntUserNic { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "FirstName")]
        public string FirstName { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "User2WorkPlaceTDES_SetAt")]
        public DateTime SetAt { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }
    }
}