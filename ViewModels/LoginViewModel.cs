using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
namespace DrinkAndGo.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_]+@gmail.com$", ErrorMessage = "Invalid Email format")]
        
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
        public string returnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogin { get; set; }
     
    }
}
