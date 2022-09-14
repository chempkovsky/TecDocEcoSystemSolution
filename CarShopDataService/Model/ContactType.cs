using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;

namespace TecDocEcoSystemDbClassLibrary
{
    public class ContactType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "ContactTypeId")]
        public int ContactTypeId { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "ContactTypeDescription")]
        public string ContactTypeDescription { get; set; }
    }
}