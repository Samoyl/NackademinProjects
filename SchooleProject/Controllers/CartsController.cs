using Microsoft.AspNetCore.Mvc;
using SchooleProject.Data;
using SchooleProject.Models;
using SchooleProject.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace SchooleProject.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly CartServices cartServices;

        public CartsController(ApplicationDbContext context, CartServices cartServices)
        {
            this.context = context;
            this.cartServices = cartServices;
        }

        // GET: Carts
        public IActionResult Index()
        {
            Cart cart = new Cart();
            var jsonCart = HttpContext.Session.GetString("CartSession");
            if (jsonCart != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
            }
            return View(cart);
        }

        public IActionResult Create(int id)
        {
            Cart cart = cartServices.AddToCart(HttpContext, id);
            return RedirectToAction(nameof(Index));

        }


        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var cartItem = cartServices.findDetails(HttpContext, id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return View(cartItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection form)
        {
            Cart cart = new Cart();
            cart = cartServices.EditDishInCart(id, HttpContext, form);
            return View(cart);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var cart = cartServices.RemovFromCart(HttpContext, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
