using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class StreetType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "StreetTypeId")]
        public int StreetTypeId { get; set; }

        [Required]
        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "StreetTypeDescription")]
        public string StreetTypeDescription { get; set; }
    }
}