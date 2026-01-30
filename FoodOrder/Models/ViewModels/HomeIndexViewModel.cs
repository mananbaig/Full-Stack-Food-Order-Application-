using System.Collections.Generic;
using FoodOrder.Models;

namespace FoodOrder.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<FoodItem> Featured { get; set; } = new List<FoodItem>();
        public IEnumerable<FoodItem> Latest { get; set; } = new List<FoodItem>();
    }
}
