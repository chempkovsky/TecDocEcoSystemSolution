using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryItemTmp
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryId")]
        public int CategoryItemId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryId")]
        public int CategoryId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemDescriptionTDES_EntCategoryItemDescription")]
        public string EntCategoryItemDescription { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemTmp_EntCategoryDescription")]
        public string EntCategoryDescription { get; set; }


    }
}