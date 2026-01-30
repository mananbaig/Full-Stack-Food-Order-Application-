namespace FoodOrder.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int CartId { get; set; }
        public int FoodId { get; set; }

        public string FoodName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }

}
