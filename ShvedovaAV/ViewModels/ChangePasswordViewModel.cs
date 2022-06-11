using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string? OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string? NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? NewPasswordConfirm { get; set; }
    }
}
