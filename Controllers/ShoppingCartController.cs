using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace DrinkAndGo.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        private readonly IDrinkRepository drinkRepository;
        private readonly ShoppingCart shoppingCart;

        public ShoppingCartController(IDrinkRepository drinkRepository, ShoppingCart shoppingCart)
        {
            this.drinkRepository = drinkRepository;
            this.shoppingCart = shoppingCart;
        }
        public ViewResult ShoppingCart()
        {
            
            var items = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = items;
            var model = new ShoppingCartViewModel()
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal()
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult AddToShoppingCart(int drinkId)
        {
            var Drink = drinkRepository.Drinks.FirstOrDefault(d => d.DrinkId == drinkId);
            if(Drink != null)
            {
                shoppingCart.AddToCart(Drink, 1);

            }
            else
            {
                ViewBag.ErrorMessage = $"Drink has id :{drinkId} is not true";
                return View("Error");
                
            }
            return RedirectToAction("ShoppingCart");
        }
        [HttpGet]
        public RedirectToActionResult RemoveFromShoppingCart(int drinkId)
        {
            var Drink = drinkRepository.Drinks.FirstOrDefault(d => d.DrinkId == drinkId);
            if (Drink != null)
            {
                shoppingCart.RemoveFromCart(Drink);

            }
            return RedirectToAction("ShoppingCart");
        }


    }
}