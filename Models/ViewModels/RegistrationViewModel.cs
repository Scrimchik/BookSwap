using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookSwap.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [MaxLength(10, ErrorMessage = "Логин слишком длинный")]
        [Remote(action: "CanRegistration", controller: "Account", ErrorMessage = "Данный логин уже используется")]
        public string Name { get; set; }
        [MinLength(5, ErrorMessage = "Слишком короткий пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Подведите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
