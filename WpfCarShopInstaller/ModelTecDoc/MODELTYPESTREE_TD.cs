using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarShopDataService.Properties;


namespace TecDocEcoSystemDbClassLibrary
{
    public class MODELTYPESTREE_TD
    {
        [Key]
        [Display(ResourceType = typeof(Resources), Name = "TECDOC_STR_ID")]
        public int STR_ID { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "TECDOC_TEX_TEXT")]
        public string TEX_TEXT { get; set; }

        public bool isOpen { get; set; }

        public virtual ICollection<MODELTYPESTREE_TD> Subitems { get; set; }
    }

    public class MODELTYPESTREE_PARENT_TD
    {
        public int STR_ID { get; set; }
        public string TEX_TEXT { get; set; }
        public int PARENT_ID { get; set; }
    }

}