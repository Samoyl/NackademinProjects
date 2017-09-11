using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchooleProject.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Dish> categoryDishes { get; set; }
    }
}
