using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryItemTecDocDescriptionTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescriptionId")]
        public int EntCategoryItemDescriptionId { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescription")]
        public string EntCategoryItemDescription { get; set; }
    }
}