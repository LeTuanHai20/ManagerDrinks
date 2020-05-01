using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinkAndGo.Controllers
{
     [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IDrinkRepository drinkRepository;

        public HomeController(IDrinkRepository drinkRepository)
        {
            this.drinkRepository = drinkRepository;
        }
        public IActionResult Index()
        {
            var model = new HomeViewModel()
            {
                PreferredDrinks = drinkRepository.PreferredDrinks
            };
            return View(model);
        }
    }
}