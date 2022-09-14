using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
    using CarShopDataService.Properties;
    public class GuestProfileTDES
    {
        [Key]
        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "GuestProfileTDES_GestUserNic")]
        public string GestUserNic { get; set; }

        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "FirstName")]
        public string FirstName { get; set; }
        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "LastName")]
        public string LastName { get; set; }
        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "GuestProfileTDES_Contact")]
        public string Contact { get; set; }

        [StringLength(160)]
        [Display(ResourceType = typeof(Resources), Name = "GuestProfileTDES_Address")]
        public string Address { get; set; }
    }


}