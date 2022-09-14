// mvcForum.Web.ViewModels.RegisterModel
using MVCBootstrap.Web.Mvc.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace mvcForum.Web.ViewModels
{

    public class RegisterModel
    {
        [LocalizedDisplay("MVCForum.Web.ViewModels.RegisterModel", "Username")]
        [Required]
        public string Username
        {
            get;
            set;
        }

        [DataType(DataType.EmailAddress)]
        [Required]
        [LocalizedDisplay("MVCForum.Web.ViewModels.RegisterModel", "EmailAddress")]
        public string EmailAddress
        {
            get;
            set;
        }

        [LocalizedDisplay("MVCForum.Web.ViewModels.RegisterModel", "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }

        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [LocalizedDisplay("MVCForum.Web.ViewModels.RegisterModel", "ConfirmPassword")]
        public string ConfirmPassword
        {
            get;
            set;
        }

        public bool RulesAccepted
        {
            get;
            set;
        }

        public bool RequireRulesAccept
        {
            get;
            set;
        }

        public string SignUpRules
        {
            get;
            set;
        }
    }
}
