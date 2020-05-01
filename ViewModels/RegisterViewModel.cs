using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_]+@gmail.com$", ErrorMessage = "Invalid Email format")]
        [Remote(action: "IsEmailInUse", controller: "account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Two password do not match")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
