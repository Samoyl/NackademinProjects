using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchooleProject.Models.Entities
{
    public class DishIngredentView
    {
        public Dish dish { get; set; }
        public IEnumerable<Ingredient> ingredient { get; set; }
    }
}
