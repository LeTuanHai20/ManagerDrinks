using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DrinkController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DrinkController(ICategoryRepository categoryRepository, IDrinkRepository drinkRepository, IWebHostEnvironment webHostEnvironment )
        {
            _categoryRepository = categoryRepository;
            _drinkRepository = drinkRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult List(string category)
        {

            IEnumerable<Drink> drinks;
            string currentCategory = string.Empty;
            if (string.IsNullOrEmpty(category))
            {
                drinks = _drinkRepository.Drinks.OrderBy(d => d.DrinkId);
                currentCategory = "All Drink";
            }
            else
            {
                if (string.Equals("Alcoholic", category, StringComparison.OrdinalIgnoreCase))
                {
                    drinks = _drinkRepository.Drinks
                        .Where(p => p.Category.CategoryName == "Alcoholic").OrderBy(p => p.Name);
                }
                else
                {
                    drinks = _drinkRepository.Drinks
                       .Where(p => p.Category.CategoryName == "Non-alcoholic").OrderBy(p => p.Name);
                }
                currentCategory = category;

            }
            var model = new DrinksViewModel()
            {
                Drinks = drinks,
                CurrentCategory = currentCategory
            };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detail(int drinkId)
        {
            var drink = _drinkRepository.Drinks.FirstOrDefault(d => d.DrinkId == drinkId);
            if (drink == null)
            {
                ViewBag.ErrorMessage = $"the drink has id : {drinkId} not found";
                return View("Error");
            }
            ViewBag.drinkId = drinkId;
            var model = new DetailViewModel()
            {
                Name = drink.Name,
                LongDescription = drink.LongDescription,
                ImageUrl = drink.ImageUrl
            };
            return View(model);
        }
      
        public IActionResult ManagerDrinks()
        {
            IEnumerable<Drink> drinks;
            drinks = _drinkRepository.Drinks.OrderBy(d => d.DrinkId);

            var model = new DrinksViewModel()
            {
                Drinks = drinks,
                CurrentCategory = "All Drink"
            };
            return View(model);
        }
        [HttpGet]
        
        public IActionResult Delete(int drinkId)
        {
            _drinkRepository.Delete(drinkId);
            return RedirectToAction("ManagerDrinks");
        }
       
        public IActionResult Create()
        {
            return View();
        }
        private List<string> ProcessUploadedFile(CreateDrinkViewModel model1, CreateDrinkViewModel model2)
        {
            string uniqueFileName1 = null;
            string uniqueFileName2 = null;
          
            if (model1.ImageUrl != null)
            {
                string uploadFile = Path.Combine(webHostEnvironment.WebRootPath, "ImageUrl");
                uniqueFileName1 = Guid.NewGuid().ToString() + model1.ImageUrl.FileName;
                string filePath1 = Path.Combine(uploadFile, uniqueFileName1);
                using (var fileStream1 = new FileStream(filePath1, FileMode.Create))
                model1.ImageUrl.CopyTo(fileStream1);     
            }
            if (model2.ImageThumbnailUrl != null)
            {
                string uploadFile = Path.Combine(webHostEnvironment.WebRootPath, "ImageThumbnailUrl");
                uniqueFileName2 = Guid.NewGuid().ToString() + model2.ImageThumbnailUrl.FileName;
                string filePath2 = Path.Combine(uploadFile, uniqueFileName2);
                using (var fileStream2 = new FileStream(filePath2, FileMode.Create))
                model2.ImageThumbnailUrl.CopyTo(fileStream2);
            }
            List<string> uniqueFileName = new List<string>()
            {
                uniqueFileName1,
                uniqueFileName2
            };
            return uniqueFileName;
        }
        [HttpPost]
        public IActionResult Create(CreateDrinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<string> uniqueFileName = ProcessUploadedFile(model, model);
                string uniqueFileName1 = uniqueFileName[0];
                string uniqueFileName2 = uniqueFileName[1];
                Drink drink = new Drink()
                {
                    Name = model.Name,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    Price = model.Price,
                    ImageUrl = uniqueFileName1,
                    ImageThumbnailUrl = uniqueFileName2,
                    IsPreferredDrink = model.IsPreferredDrink,
                    InStock = model.InStock,
                    CategoryId = model.CategoryId
                };
                _drinkRepository.Add(drink);
                return RedirectToAction("ManagerDrinks");
            }
            return View(model);
        }

    }
}
