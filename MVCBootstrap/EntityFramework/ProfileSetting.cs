//using System;
//using System.ComponentModel.DataAnnotations;

//namespace MVCBootstrap.Membership {

//    public class ProfileSetting {
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public Guid Id { get; set; }
//        [Required]
//        public Profile Profile { get; set; }
//        [Required]
//        [StringLength(256)]
//        public String Name { get; set; }
//        [StringLength(Int32.MaxValue)]
//        public String StringValue { get; set; }
//        public Byte[] BinaryValue { get; set; }
//    }
//}