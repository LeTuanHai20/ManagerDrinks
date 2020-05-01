using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class OrderDetailViewModel
    {
        public int DrinkId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
