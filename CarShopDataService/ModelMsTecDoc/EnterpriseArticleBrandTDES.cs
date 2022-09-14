using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseArticleBrandTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleBrandTDES_ArticleBrandId")]
        public int ArticleBrandId { get; set; }

        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseArticleBrandTDES_ArticleBrandNic")]
        public string ArticleBrandNic { get; set; }
    }
}