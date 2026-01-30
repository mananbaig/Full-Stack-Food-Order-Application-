namespace FoodOrder.Models.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userID);
        Task<User> GetUserByEmailAsync(string email);
        Task<int> GetTotalOrdersAsync(string userId);
        Task<decimal> GetTotalSpentAsync(string userId);
        Task<int> GetTotalItemsOrderedAsync(string userId);
        Task<int> GetPendingOrdersAsync(string userId);
    }
}
