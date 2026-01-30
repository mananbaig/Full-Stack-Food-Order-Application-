namespace FoodOrder.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalItemsOrdered { get; set; }
        public int PendingOrders { get; set; }
    }
}
