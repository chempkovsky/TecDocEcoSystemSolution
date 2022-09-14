using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseCarBrandTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarBrandId")]
        public int EnterpriseCarBrandId { get; set; }

        [Required]
        [StringLength(80)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseCarBrandName")]
        public string EnterpriseCarBrandName { get; set; }

        public virtual ICollection<EnterpriseCarModelTypeTDES> EnterpriseCarModelTypeTDESes { get; set; }
    }
}
