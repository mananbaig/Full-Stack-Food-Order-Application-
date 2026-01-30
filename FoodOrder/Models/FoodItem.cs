namespace FoodOrder.Models
{
    public class FoodItem
    {
        public int FoodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool isAvailable { get; set; }

    }
}
