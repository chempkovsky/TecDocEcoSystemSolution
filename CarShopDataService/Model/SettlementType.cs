using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;



namespace TecDocEcoSystemDbClassLibrary
{
    public class SettlementType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "SettlementTypeId")]
        public int SettlementTypeId { get; set; }

        [Required]
        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "SettlementTypeDescription")]
        public string SettlementTypeDescription { get; set; }

        [Required]
        [StringLength(15)]
        [Display(ResourceType = typeof(Resources), Name = "SettlementTypeShortDescription")]
        public string SettlementTypeShortDescription { get; set; }
    }
}