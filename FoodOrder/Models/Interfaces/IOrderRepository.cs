namespace FoodOrder.Models.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
        Task<int> GetCount();
    }
}
