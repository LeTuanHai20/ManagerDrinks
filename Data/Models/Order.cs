using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Models
{
    public class Order
    {
        [Required]
        public int OrderId { get; set; }
        public string Email { get; set; }
        [Required]
        public List<OrderDetail> OrderLines { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public string NoteToSalesMan { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime OrderPlace { get; set; }
    }
}
