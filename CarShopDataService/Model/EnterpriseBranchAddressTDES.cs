using CarShopDataService.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecDocEcoSystemDbClassLibrary;

namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseBranchAddressTDES : Address
    {
        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }

        [Required]
//        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }
        public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

    }
}
