using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchooleProject.Data;
using SchooleProject.Models;
using SchooleProject.Services;

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
        public IActionResult Index(int id)
        {
            Cart cart = cartServices.AddToCart(HttpContext, id);

            return View(cart);
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await context.Cart.SingleOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                context.Add(cart);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await context.Cart.SingleOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id")] Cart cart)
        {
            if (id != cart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(cart);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            //var cart = await context.Cart
            //    .SingleOrDefaultAsync(m => m.id == id);
            var cart = cartServices.RemovFromCart(HttpContext, id);
            //if (cart == null)
            //{
            //    return NotFound();
            //}

            return RedirectToAction(nameof(Index));
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await context.Cart.SingleOrDefaultAsync(m => m.id == id);
            context.Cart.Remove(cart);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return context.Cart.Any(e => e.id == id);
        }
    }
}
