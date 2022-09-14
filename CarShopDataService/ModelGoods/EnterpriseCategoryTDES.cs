using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryTDES
    {

        [Key, Column(Order = 0)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryId")]
        public int CategoryId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryDescription")]
        [StringLength(120)]
        public string CategoryDescription { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryParent")]
        public int CategoryParent { get; set; }

        public virtual ICollection<EnterpriseCategoryItemTDES> EnterpriseCategoryItemTDESes { get; set; }
    }
}