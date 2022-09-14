using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TecDocEcoSystemDbClassLibrary
{
    using CarShopDataService.Properties;


    public class EnterpriseBranchReplyTDES
    {
        [Key, Column(Order = 0)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchTDES_EntBranchGuid")]
        public Guid EntBranchGuid { get; set; }
        public virtual EnterpriseBranchTDES EnterpriseBranchTDES { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_ReplyType")]
        public int ReplyType { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_BaseHttpAddress")]
        public string BaseHttpAddress { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpLoginUrl")]
        public string HttpLoginUrl { get; set; }


        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpGetUrl")]
        public string HttpGetUrl { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpPostUrl")]
        public string HttpPostUrl { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpPutUrl")]
        public string HttpPutUrl { get; set; }

        [Required]
        [StringLength(120)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpDeleteUrl")]
        public string HttpDeleteUrl { get; set; }

        [Required]
        [StringLength(30)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpUser")]
        public string HttpUser { get; set; }

        [Required]
        [StringLength(30)]
        [Display(ResourceType = typeof(Resources), Name = "EnterpriseBranchReplyTDES_HttpPassword")]
        public string HttpPassword { get; set; }

        
    }
}