using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class AddressType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "AddressTypeId")]
        public int AddressTypeId { get; set; }

        [Required]
        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "AddressTypeDescription")]
        public string AddressTypeDescription { get; set; }
    }
}