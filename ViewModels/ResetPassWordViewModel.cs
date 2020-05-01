﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class ResetPassWordViewModel
    {

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9_]+@gmail.com$", ErrorMessage = "Invalid Email format")]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage = "Password and ConfirmPassword must match")]
        [Display(Name ="Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
