using System.Collections.Generic;
using System.Threading.Tasks;
using FoodOrder.Models;

namespace FoodOrder.Models.Interfaces
{
    public interface IFoodRepository
    {
        Task<IEnumerable<FoodItem>> GetAllAsync();
        Task<FoodItem?> GetByIdAsync(int id);
        Task<int> GetCount();
        Task AddAsync(FoodItem item);
        Task UpdateAsync(FoodItem item);
        Task DeleteAsync(int id);

    }
}
