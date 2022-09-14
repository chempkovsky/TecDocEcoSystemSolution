// CarShop.Models.ManageUserViewModel
using System.ComponentModel.DataAnnotations;

namespace CarShop.Models
{

    public class ManageUserViewModel
    {
        [Display(Name = "Текущий пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword
        {
            get;
            set;
        }

        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Required]
        public string NewPassword
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword
        {
            get;
            set;
        }
    }
}