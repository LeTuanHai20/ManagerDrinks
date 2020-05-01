using DrinkAndGo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class OrderViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string NoteToSalesMan { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public ShoppingCart shoppingCart { get; set; }
    }
}
