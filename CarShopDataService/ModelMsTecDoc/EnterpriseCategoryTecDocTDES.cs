using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCategoryTecDocTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryId")]
        public int CategoryId { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryDescription")]
        [StringLength(120)]
        public string CategoryDescription { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCategoryTDES_CategoryParent")]
        public int CategoryParent { get; set; }

        public virtual ICollection<EnterpriseCategoryItemTecDocTDES> EnterpriseCategoryItemTecDocTDESes { get; set; }
    }
}