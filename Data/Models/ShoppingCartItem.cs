using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Models
{
    public class ShoppingCartItem
    {
        [Required]
        public int ShoppingCartItemId { get; set; }
        public Drink Drink { get; set; }
        public int Amount { get; set; }
        [Required]
        public string ShoppingCartId { get; set; }
    }
}
