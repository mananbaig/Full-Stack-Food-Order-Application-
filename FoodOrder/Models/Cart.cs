namespace FoodOrder.Models
{
    public class Cart
    {
        public int CartId { get; set; }         
        public string UserId { get; set; }          
        public int FoodId { get; set; }           
        public int Quantity { get; set; }
        public string FoodName { get; set; }
        public decimal Price { get; set; }
    }
}
