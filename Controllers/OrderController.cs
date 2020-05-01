using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Models;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrinkAndGo.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;

        }
        [Authorize]
        public IActionResult CheckOut()
        {
            string email = User.Identity.Name;
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var model = new OrderViewModel()
            {
                Email = email,
                shoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Checkout(OrderViewModel model)
        {
            _shoppingCart.ShoppingCartItems = _shoppingCart.GetShoppingCartItems();
            if(_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your card is empty, add some drinks first");
            }

            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    NoteToSalesMan = model.NoteToSalesMan,
                    TotalPrice = _shoppingCart.GetShoppingCartTotal()
                };
                _orderRepository.Create(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CheckoutComplete()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult ListOrders()
        {
            
            List<ListOrderViewModel> models = new List<ListOrderViewModel>();
            foreach (var order in _orderRepository.GetOrders())
            {
                var model = new ListOrderViewModel()
                {
                    OrderId = order.OrderId,
                    Email = order.Email,
                    FullName = order.FullName,
                    PhoneNumber = order.PhoneNumber,
                    Address = order.Address,
                    TotalPrice = order.TotalPrice,
                    NoteToSalesMan = order.NoteToSalesMan,
                    OrderPlace = order.OrderPlace
                };
                models.Add(model);
            }
            return View(models);
        }
        public IActionResult RemoveOrder(int id)
        {
            _orderRepository.RemoveOrder(id);
            return RedirectToAction("listOrders");
        }
        public IActionResult WatchOrderDetail(int id)
        {
            List<OrderDetailViewModel> models = new List<OrderDetailViewModel>();
             foreach (var detail in _orderRepository.GetOrderDetails(id))
                {
                OrderDetailViewModel model = new OrderDetailViewModel()
                {
                    DrinkId = detail.DrinkId,
                    Amount = detail.Amount,
                    Price = detail.Price
                };
                 models.Add(model);
                }
            return View(models);
        }

    }
}