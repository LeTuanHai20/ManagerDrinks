using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class EditUserViewModel
    {

        [Display(Name ="User ID")]
        public string Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_]+@gmail.com$", ErrorMessage = "Invalid Email format")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name ="User name")]
        public string UserName { get; set; }
    }
}
