using DrinkAndGo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Interfaces
{
    public interface IOrderRepository
    {
        void Create(Order order);
        IEnumerable<Order> GetOrders();
        Order RemoveOrder(int id);
        IEnumerable<OrderDetail> GetOrderDetails(int id);
    }

}
