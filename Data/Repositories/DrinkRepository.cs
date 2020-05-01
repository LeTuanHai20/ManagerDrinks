using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data.Repositories
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly AppDbContext _context;

        public DrinkRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Drink> Drinks => _context.Drinks.Include(c => c.Category);



        public IEnumerable<Drink> PreferredDrinks => _context.Drinks.Where(p => p.IsPreferredDrink).Include(c => c.Category);

        public Drink GetDrinkById(int drinkId)
        {
            var drink = _context.Drinks.Find(drinkId);
            return drink;
        }
        public Drink Delete(int drinkId)
        {
            
            List<ShoppingCartItem> shoppingCartItems = _context.ShoppingCartItems.Where(sp => sp.Drink.DrinkId == drinkId).ToList();
            if (shoppingCartItems.Count > 0)
            {
                _context.ShoppingCartItems.RemoveRange(shoppingCartItems);
                _context.SaveChanges();
            }
   
            Drink drink = _context.Drinks.Find(drinkId);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
                _context.SaveChanges();
            }
            return drink;
        }

        public Drink Add(Drink drink)
        {
            if(drink != null)
            {
                _context.Drinks.Add(drink);
                _context.SaveChanges();
            }
            return drink;
        }
    }
}
