using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Эл.Адрес")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}
