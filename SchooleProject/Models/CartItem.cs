using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchooleProject.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public Dish dishItem { get; set; }
        public Cart cart { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Ingredient> ExtraIngredients { get; set; } = new List<Ingredient>();
    }
}
