﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchooleProject.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [Required]
        public string Name { get; set; }
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
