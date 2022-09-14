using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
    using CarShopDataService.Properties;

    public class GuestBranchTDES
    {
        [Key]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EntBranchDescription")]
        public string EntBranchDescription { get; set; }

        [StringLength(15)]
        [Display(ResourceType = typeof(Resources), Name = "AddressPostCode")]
        public string AddressPostCode { get; set; }


        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressRegion")]
        public string AddressRegion { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressSettlementName")]
        public string AddressSettlementName { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressStreetName")]
        public string AddressStreetName { get; set; }

        // долгота
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Display(ResourceType = typeof(Resources), Name = "AddressLongitude")]
        public double AddressLongitude { get; set; }

        // широта
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Display(ResourceType = typeof(Resources), Name = "AddressLatitude")]
        public double AddressLatitude { get; set; }



        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "WorkingDays")]
        public string WorkingDays { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "WorkingTime")]
        public string WorkingTime { get; set; }

    }
}