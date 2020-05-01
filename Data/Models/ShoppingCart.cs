using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _context;

        [Required]
        public string _ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ShoppingCart(AppDbContext context)
        {
           _context = context;
        }
        public static ShoppingCart GetCart(IServiceProvider service)
        {
            
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = service.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            
            return new ShoppingCart(context) {  _ShoppingCartId = cartId };
        }

        public void AddToCart(Drink drink, int amount)
        {
            var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
                s => s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == _ShoppingCartId);
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = _ShoppingCartId,
                    Drink = drink,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }
        public int RemoveFromCart(Drink drink)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                .SingleOrDefault(s => s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == _ShoppingCartId);
            int localAmount = 0;
            if(shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount --;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _context.SaveChanges();

            return localAmount;

        }
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            var cart  = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == _ShoppingCartId)
                           .Include(s => s.Drink)
                           .ToList();
            return ShoppingCartItems ?? cart;
                  
                       
        }
        public void ClearCart()
        {
            var cartItems = _context
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == _ShoppingCartId);

            _context.ShoppingCartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == _ShoppingCartId)
                .Select(c => c.Drink.Price * c.Amount).Sum();
            return total;
        }
    }

}
