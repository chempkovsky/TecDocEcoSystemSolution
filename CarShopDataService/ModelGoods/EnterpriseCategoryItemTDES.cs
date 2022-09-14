using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryItemTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemTDES_CategoryItemId")]
        public int CategoryItemId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryId")]
        public int CategoryId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Required]
        public virtual EnterpriseCategoryTDES EnterpriseCategory { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemTDES_EntCategoryItemDescriptionId")]
        public int EntCategoryItemDescriptionId { get; set; }
        public virtual EnterpriseCategoryItemDescriptionTDES EnterpriseCategoryItemDescription { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemTDES_EntCategoryDescriptionId")]
        public int EntCategoryDescriptionId { get; set; }
        public virtual EnterpriseCategoryDescriptionTDES EnterpriseCategoryDescription { get; set; }

    }
}