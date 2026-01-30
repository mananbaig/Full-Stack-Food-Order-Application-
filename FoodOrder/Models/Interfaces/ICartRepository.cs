using System.Collections.Generic;
using System.Threading.Tasks;
using FoodOrder.Models.ViewModels;

namespace FoodOrder.Models.Interfaces
{
    public interface ICartRepository
    {
        Task AddToCartAsync(string userId, int foodId, int quantity);
        Task<IEnumerable<CartItemViewModel>> GetCartByUserAsync(string userId);
        Task RemoveFromCartAsync(int cartID);
    }
}
