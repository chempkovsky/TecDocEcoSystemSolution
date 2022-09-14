using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class Soato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [StringLength(10, MinimumLength = 10)]
        [Display(ResourceType = typeof(Resources), Name = "SoatoId")]
        public string SoatoId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "SoatoSettlementName")]
        public string SoatoSettlementName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "SoatoSettlementType")]
        public int SettlementTypeId { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "SettlementTypeDescription")]
        public virtual SettlementType SettlementType { get; set; }
    }
}