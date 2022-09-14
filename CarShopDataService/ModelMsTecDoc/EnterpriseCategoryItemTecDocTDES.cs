using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryItemTecDocTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemTDES_CategoryItemId")]
        public int CategoryItemId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryId")]
        public int CategoryId { get; set; }
        public virtual EnterpriseCategoryTecDocTDES EnterpriseCategory { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryItemTDES_EntCategoryItemDescriptionId")]
        public int EntCategoryItemDescriptionId { get; set; }
        public virtual EnterpriseCategoryItemTecDocDescriptionTDES EnterpriseCategoryItemTecDocDescription { get; set; }

    }
}