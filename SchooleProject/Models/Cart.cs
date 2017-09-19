using System.Collections.Generic;
namespace SchooleProject.Models
{
    public class Cart
    {
        public int id { get; set; }
        public List<CartItem> CartItem { get; set; } = new List<CartItem>();
    }
}