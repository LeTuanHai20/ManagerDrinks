using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(AppDbContext context,ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public void Create(Order order)
        {
            order.OrderPlace = DateTime.Now;
            _context.Orders.Add(order);
            _context.SaveChanges();
            var shoppingCartItems = _shoppingCart.ShoppingCartItems;
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    DrinkId = shoppingCartItem.Drink.DrinkId,
                    OrderId = order.OrderId,
                    Price = shoppingCartItem.Drink.Price
                };

                _context.OrderDetails.Add(orderDetail);
            }
                _context.SaveChanges();

        }

        public IEnumerable<OrderDetail> GetOrderDetails(int id)
        {
            List<OrderDetail> orderDetails = _context.OrderDetails.Where(o => o.OrderId == id).ToList();
            return orderDetails;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders;
        }

        public Order RemoveOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if(order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return order;
        }
    }
}
