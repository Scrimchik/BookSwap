using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookSwap.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [Remote(action: "CanLogin", controller: "Account", ErrorMessage = " ", AdditionalFields = "Password")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [Remote(action: "CanLogin", controller: "Account", ErrorMessage = "Неправильное имя пользователя или пароль", AdditionalFields = "Name")]
        public string Password { get; set; }
    }
}
