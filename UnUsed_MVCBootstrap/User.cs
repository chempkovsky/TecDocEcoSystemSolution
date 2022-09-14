// MVCBootstrap.User
using MVCBootstrap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBootstrap
{

    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get;
            set;
        }

        [Required]
        [StringLength(256)]
        public string Username
        {
            get;
            set;
        }

        [StringLength(200)]
        [Required]
        public string EmailAddress
        {
            get;
            set;
        }

        [Required]
        [StringLength(100)]
        public string Password
        {
            get;
            set;
        }

        [Required]
        public DateTime Created
        {
            get;
            set;
        }

        [Required]
        public DateTime LastVisit
        {
            get;
            set;
        }

        [Required]
        public bool Locked
        {
            get;
            set;
        }

        [Required]
        public bool Approved
        {
            get;
            set;
        }

        [Required]
        public DateTime LastPasswordFailure
        {
            get;
            set;
        }

        [Required]
        public int PasswordFailures
        {
            get;
            set;
        }

        [Required]
        public DateTime LastLockout
        {
            get;
            set;
        }

        public virtual ICollection<Role> Roles
        {
            get;
            set;
        }

        public User()
        {
            Roles = new List<Role>();
        }
    }

}
