using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class DetailViewModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string LongDescription { get; set; }
    }
}
