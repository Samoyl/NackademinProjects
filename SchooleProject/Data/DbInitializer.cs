using Microsoft.AspNetCore.Identity;
using SchooleProject.Models;
using SchooleProject.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SchooleProject.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (context.Dishes.ToList().Count == 0)
            {
                var pizza = new Category { name = "Pizza" };
                var pasta = new Category { name = "Pasta" };
                var salad = new Category { name = "Salad" };

                var cheese = new Ingredient { Name = "Cheese" };
                var tomatoe = new Ingredient { Name = "Tomatoe" };
                var ham = new Ingredient { Name = "Ham" };
                var lok = new Ingredient { Name = "Lök" };

                var capricciosa = new Dish { Name = "Capricciosa", Price = 79, category = pizza };
                var margaritha = new Dish { Name = "Margaritha", Price = 69, category = pizza };
                var hawaii = new Dish { Name = "Hawaii", Price = 85, category = pizza };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomatoe = new DishIngredient { Dish = capricciosa, Ingredient = tomatoe };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };
                var capricciosaLok = new DishIngredient { Dish = capricciosa, Ingredient = lok };

                var margarithaHam = new DishIngredient { Dish = margaritha, Ingredient = ham };
                var margarithaTomatoe = new DishIngredient { Dish = margaritha, Ingredient = tomatoe };
                var hawaiiChess = new DishIngredient { Dish = hawaii, Ingredient = cheese };

                capricciosa.DishIngredients = new List<DishIngredient> { capricciosaTomatoe, capricciosaCheese, capricciosaHam, capricciosaLok };
                margaritha.DishIngredients = new List<DishIngredient> { margarithaHam, margarithaTomatoe };
                hawaii.DishIngredients = new List<DishIngredient> { hawaiiChess };
                context.Dishes.AddRange(capricciosa, margaritha, hawaii);
                context.Category.AddRange(pizza, pasta, salad);
                context.SaveChanges();
            }
        }
    }
}