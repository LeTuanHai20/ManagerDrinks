using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class CreateDrinkViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name= "Short Description")]
        public string ShortDescription { get; set; }
        [Required]
        [MaxLength(2000)]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Image in Detail")]
        public IFormFile ImageUrl { get; set; }
        [Required]
        [Display(Name = "Image in List")]
        public IFormFile ImageThumbnailUrl { get; set; }   
        [Display(Name = "Is Preferred Drink")]
        public bool IsPreferredDrink { get; set; }
        public bool InStock { get; set; }
        [Required]
        [Display(Name = "Type")]
        public int CategoryId { get; set; }
    }
}
