namespace FoodOrder.Models.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddOrderItemAsync(IEnumerable<OrderItem> items);
        Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId);
    }
}
