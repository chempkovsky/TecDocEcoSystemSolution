using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class EnterpriseTecDocSrcTypeTDES
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTecDocSrcTypeTDES_Id")]
        public int TecDocSrcTypeId { get; set; }

        [RequiredAttribute]
        [StringLength(60)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseTecDocSrcTypeTDES_Descr")]
        public string TecDocSrcTypeDescr { get; set; }

    }
}