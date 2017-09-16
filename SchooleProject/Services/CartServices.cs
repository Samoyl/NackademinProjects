using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchooleProject.Data;
using SchooleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchooleProject.Services
{
    public class CartServices
    {
        private readonly ApplicationDbContext context;

        public CartServices(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            this.context = context;
        }

        public Cart AddToCart(HttpContext httpContext, int id)
        {
            Cart cart = new Cart();
            List<Dish> dishes = new List<Dish>();
            var dish = context.Dishes
                .Include(ca => ca.category)
                .Include(d => d.DishIngredients)
                .SingleOrDefault(m => m.DishId == id);
            var jsonCart = httpContext.Session.GetString("CartSession");

            if (jsonCart != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
                foreach (var dishItem in cart.Dishes)
                {
                    dishes.Add(dishItem);
                    cart.Dishes = dishes;

                }
            }

            if (dish != null)
            {
                dishes.Add(dish);
                cart.Dishes = dishes;
                var cartJson = JsonConvert.SerializeObject(cart, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                httpContext.Session.SetString("CartSession", cartJson);
            }
            return cart;
        }

        public Cart RemovFromCart(HttpContext httpContext, int id)
        {
            Cart cart = new Cart();
            List<Dish> dishes = new List<Dish>();
            var dish = context.Dishes
                .Include(ca => ca.category)
                .Include(d => d.DishIngredients)
                .SingleOrDefault(m => m.DishId == id);
            var jsonCart = httpContext.Session.GetString("CartSession");

            if (jsonCart != null || dish != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
                dishes = cart.Dishes;
                cart.Dishes = null;
                for(int i = 0; i < dishes.Count; i++)
                {
                    if(dishes.ElementAt(i).DishId == dish.DishId)
                    {
                        dishes.RemoveAt(i);
                    }
                }
                
                cart.Dishes = dishes;

                var cartJson = JsonConvert.SerializeObject(cart, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                httpContext.Session.SetString("CartSession", cartJson);
            }
                
            return cart;
        }

    }
}
