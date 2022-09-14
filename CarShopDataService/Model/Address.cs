using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;



namespace TecDocEcoSystemDbClassLibrary
{
    public abstract class Address
    {
        [Key]
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "AddressGuid")]
        public Guid AddressGuid { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "AddressType")]
        [Range(1, int.MaxValue)]
        public int AddressTypeId { get; set; }
        public virtual AddressType AddressType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "CountryName")]
        public int CountryIso { get; set; }
        public virtual Country Country { get; set; }

        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressRegion")]
        public string AddressRegion { get; set; }

        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressDistrict")]
        public string AddressDistrict { get; set; }

        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressRural")]
        public string AddressRural { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "SettlementType")]
        public int SettlementTypeId { get; set; }
        public virtual SettlementType SettlementType { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressSettlementName")]
        public string AddressSettlementName { get; set; }

        [StringLength(10)]
        [Display(ResourceType = typeof(Resources), Name = "SoatoId")]
        public string SoatoId { get; set; }
        public virtual Soato Soato { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(ResourceType = typeof(Resources), Name = "StreetType")]
        public int StreetTypeId { get; set; }
        public virtual StreetType StreetType { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "AddressStreetName")]
        public string AddressStreetName { get; set; }

        [StringLength(15)]
        [Display(ResourceType = typeof(Resources), Name = "AddressHouse")]
        public string AddressHouse { get; set; }

        [StringLength(15)]
        [Display(ResourceType = typeof(Resources), Name = "AddressBuilding")]
        public string AddressBuilding { get; set; }

        [StringLength(15)]
        [Display(ResourceType = typeof(Resources), Name = "AddressOffice")]
        public string AddressOffice { get; set; }

        [StringLength(15)]
        [Display(ResourceType = typeof(Resources), Name = "AddressPostCode")]
        public string AddressPostCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")] // , HtmlEncode = true
        [Display(ResourceType = typeof(Resources), Name = "AddressValidFrom")]
        public DateTime AddressValidFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")] // , HtmlEncode = true
        [Display(ResourceType = typeof(Resources), Name = "AddressValidTo")]
        public DateTime AddressValidTo { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsVisible")]
        public bool IsVisible { get; set; }

        // долгота
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Display(ResourceType = typeof(Resources), Name = "AddressLongitude")]
        public double AddressLongitude { get; set; }

        // широта
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        [Display(ResourceType = typeof(Resources), Name = "AddressLatitude")]
        public double AddressLatitude { get; set; }

    }
}