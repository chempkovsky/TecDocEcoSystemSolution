using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryDescriptionTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryDescriptionTDES_EntCategoryDescriptionId")]
        public int EntCategoryDescriptionId { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryDescriptionTDES_EntCategoryDescription")]
        public string EntCategoryDescription { get; set; }

        public virtual ICollection<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESes { get; set; }
    }
}