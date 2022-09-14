using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCarModelTypeTDES
    {

        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelTypeTDES_EnterpriseCarBrandId")]
        public int EnterpriseCarBrandId { get; set; }
        public virtual EnterpriseCarBrandTDES EnterpriseCarBrandTDES { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelTypeId")]
        public int EnterpriseCarModelTypeId { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarModelTypeName")]
        public string EnterpriseCarModelTypeName { get; set; }

        public virtual ICollection<EnterpriseCarModelTDES> EnterpriseCarModelTDESes { get; set; }
    }
}
