using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchooleProject.Data;
using SchooleProject.Models;
using SchooleProject.Models.Entities;
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
            CartItem item = new CartItem();

            var dish = context.Dishes.Include(ca => ca.category).Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient).SingleOrDefault(m => m.DishId == id);

            var jsonCart = httpContext.Session.GetString("CartSession");

            if (jsonCart != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(jsonCart);

            }

            if (dish != null)
            {
                if(cart.CartItem.Count == 0)
                {
                    item.id = 1;
                }
                else
                {
                    item.id = cart.CartItem.ElementAt(cart.CartItem.Count - 1).id + 1;
                }
                
                item.dishItem = dish;
                item.cart = cart;
                item.price = dish.Price;
                cart.CartItem.Add(item);

                var cartJson = JsonConvert.SerializeObject(cart, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                httpContext.Session.SetString("CartSession", cartJson);
            }
            return cart;
        }

        public CartItem findDetails(HttpContext httpContext, int id)
        {
            var jsonCart = httpContext.Session.GetString("CartSession");
            var cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
            var item = cart.CartItem.Where(im => im.id == id).SingleOrDefault();
            item.Ingredients = context.Ingredients.ToList();

            return item;
        }

        public Cart EditDishInCart(int id, HttpContext httpContext, IFormCollection form)
        {

            var jsonCart = httpContext.Session.GetString("CartSession");
            var cart = JsonConvert.DeserializeObject<Cart>(jsonCart);
            var cartItem = cart.CartItem.Where(x => x.id == id).SingleOrDefault();
            cartItem.ExtraIngredients = new List<Ingredient>();
            cartItem.price = 0;
            foreach (var item in context.Ingredients)
            {
                var check = form.Keys.Any(k => k == $"{item.IngredientId}");

                if (check)
                {
                    cartItem.ExtraIngredients.Add(item);
                    cartItem.price += item.Price;
                }
            }

            cartItem.price += cartItem.dishItem.Price;
            var cartJson = JsonConvert.SerializeObject(cart, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            httpContext.Session.SetString("CartSession", cartJson);
            return cart;
        }

        public Cart RemovFromCart(HttpContext httpContext, int id)
        {
            var jsonCart = httpContext.Session.GetString("CartSession");
            var cart = new Cart();
            if (jsonCart != null)
            {
                cart = JsonConvert.DeserializeObject<Cart>(jsonCart);

                var itemToRemove = cart.CartItem.Where(x => x.id == id).SingleOrDefault();

                cart.CartItem.Remove(itemToRemove);        

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
