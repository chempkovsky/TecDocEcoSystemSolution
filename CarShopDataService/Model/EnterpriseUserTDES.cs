using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;
using System.Collections.ObjectModel;


namespace TecDocEcoSystemDbClassLibrary
{
    public partial class EnterpriseUserTDES
    {
        [Key]
        [Required]
        [StringLength(40)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseUserTDES_EntUserNic")]
        public string EntUserNic { get; set; }

        [Required]
        [StringLength(30)]
        [Display(ResourceType = typeof(Resources), Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "FirstName")]
        public string FirstName { get; set; }
        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "LastName")]
        public string LastName { get; set; }
        [StringLength(20)]
        [Display(ResourceType = typeof(Resources), Name = "MiddleName")]
        public string MiddleName { get; set; }


        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsActive")]
        public bool IsActive { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsAdmin")]
        public bool IsAdmin { get; set; }
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "IsAudit")]
        public bool IsAudit { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTDES_EntGuid")]
        public Guid EntGuid { get; set; }
        public virtual EnterpriseTDES EnterpriseTDES { get; set; }

        public virtual ICollection<EnterpriseUserContactTDES> EnterpriseUserContactTDESes { get; set; }
    }
}
