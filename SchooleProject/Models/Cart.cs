using System.Collections.Generic;
namespace SchooleProject.Models
{
    public class Cart
    {
        public int id { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}