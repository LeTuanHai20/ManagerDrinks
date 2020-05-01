using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.ViewModels
{
    public class ListOrderViewModel
    {
        public int OrderId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }    
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public string NoteToSalesMan { get; set; }
        public DateTime OrderPlace { get; set; }
    }
}
