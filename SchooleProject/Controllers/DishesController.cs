using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchooleProject.Data;
using SchooleProject.Models;
using System.Threading.Tasks;
using System.Linq;
using SchooleProject.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace SchooleProject.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext context;

        public DishesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //GET: Dieshes
        public async Task<IActionResult> Index()
        {
            var dishes = context.Dishes;
            return View(await dishes.ToListAsync());
        }

        //GET: Dishe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishes = await context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == id);

            if (dishes == null)
            {
                return NotFound();
            }
            return View(dishes);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId, Name, Price")] Dish dish)
        {
            if (dish == null)
            {
                return new BadRequestObjectResult("dish in nul");
            }
            if (ModelState.IsValid)
            {
                context.Add(dish);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", dish);
        }

        //GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            DishIngredentView modelView = new DishIngredentView();
            if (id == null)
            {
                return NotFound();
            }
            modelView.ingredient = await context.Ingredients.ToListAsync();
            modelView.dish = await context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (modelView == null)
            {
                return NotFound();
            }
            return View(modelView);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dish dish, IFormCollection form)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    RemoveIngredientsByDish(id);
                    foreach (var item in context.Ingredients)
                    {
                        var dishIngredent = new DishIngredient();
                        var checkId = form.Keys.Where(k => k == $"{item.IngredientId}").FirstOrDefault();
                        if (checkId != null)
                        {
                            dishIngredent.DishId = id;
                            dishIngredent.Dish = dish;
                            dishIngredent.IngredientId = Int32.Parse(checkId);
                            dishIngredent.Ingredient = context.Ingredients.Where(x => x.IngredientId == Int32.Parse(checkId)).FirstOrDefault();
                        }
                        context.Add(dishIngredent);
                    }
                    context.Update(dish);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.DishId))
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
            return View(dish);
        }

        public void RemoveIngredientsByDish(int id)
        {
            var dishIng = context.DishIngredients.Where(d => d.DishId == id);
            foreach (var item in dishIng)
            {
                context.Remove(item);

            }
            context.SaveChanges();
        }
        //GET: Dieshes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dish = await context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            context.Dishes.Remove(dish);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return context.Dishes.Any(e => e.DishId == id);
        }
    }
}
