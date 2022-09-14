using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCarModelFuelTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TYP_KV_FUEL")]
        public int FUELId { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "FuelName")]
        public string FuelName { get; set; }

        public virtual ICollection<EnterpriseCarModelTDES> EnterpriseCarModelTDESes { get; set; }
    }
}