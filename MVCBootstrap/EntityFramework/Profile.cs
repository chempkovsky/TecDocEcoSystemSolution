//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace MVCBootstrap.Membership {

//    public class Profile {
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public Guid Id { get; set; }
//        [Required]
//        [StringLength(256)]
//        public String Username { get; set; }
//        [Required]
//        public DateTime LastUpdated { get; set; }

//        public virtual ICollection<ProfileSetting> Settings { get; set; }
//    }
//}