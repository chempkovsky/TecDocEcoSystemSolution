using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "CountryIso")]
        public int CountryIso { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        [Display(ResourceType = typeof(Resources), Name = "CountryCode2")]
        public string CountryCode2 { get; set; }

        [Required]
        [StringLength(3, MinimumLength=3)]
        [Display(ResourceType = typeof(Resources), Name = "CountryCode3")]
        public string CountryCode3 { get; set; }


        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "CountryName")]
        public string CountryName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "CountryEngName")]
        public string CountryEngName { get; set; }

        
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "CountryCapital")]
        public string CountryCapital { get; set; }

        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "CountryPhone")]
        public string CountryPhone { get; set; }

    }
}