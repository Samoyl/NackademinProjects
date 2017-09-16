using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchooleProject.Models
{
    public class Category
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public List<Dish> categoryDishes { get; set; }
    }
}
