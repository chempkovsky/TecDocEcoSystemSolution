using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class Currency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "CurrencyIso")]
        public int CurrencyIso { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        [Display(ResourceType = typeof(Resources), Name = "CurrencyCode3")]
        public string CurrencyCode3 { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "CurrencyName")]
        public string CurrencyName { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "FractionalUnit")]
        public int FractionalUnit { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "FractionalUnitName")]
        public string FractionalUnitName { get; set; }
        
        [StringLength(50)]
        [Display(ResourceType = typeof(Resources), Name = "IssuerName")]
        public string IssuerName { get; set; }
        
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsNational")]
        public bool IsNational { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsOperational")]
        public bool IsOperational { get; set; }

    }
}

