using Dapper;
using global::FoodOrder.Models.ViewModels;
using FoodOrder.Models.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace FoodOrder.Models.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodOrder;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public async Task AddToCartAsync(string userId, int foodId, int quantity)
        {
            using var connection = new SqlConnection(_connectionString);
            var existing = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Cart WHERE UserId = @UserId AND FoodId = @FoodId",
                new { UserId = userId, FoodId = foodId });
            if (existing > 0)
            {
                await connection.ExecuteAsync(
                    "UPDATE Cart SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND FoodId = @FoodId",
                    new { Quantity = quantity, UserId = userId, FoodId = foodId });
            }
            else
            {
                await connection.ExecuteAsync(
                    "INSERT INTO Cart (UserId, FoodId, Quantity) VALUES (@UserId, @FoodId, @Quantity)",
                    new { UserId = userId, FoodId = foodId, Quantity = quantity });
            }
        }

        public async Task<IEnumerable<CartItemViewModel>> GetCartByUserAsync(string userId)
        {
            using var conn = new SqlConnection(_connectionString);

            string sql = @"
                SELECT 
                c.CartId,
                c.FoodId,
                c.Quantity,
                f.Name AS FoodName,
                f.Price,
                f.ImageUrl
               FROM Cart c
               INNER JOIN FoodItem f ON c.FoodId = f.FoodId
               WHERE c.UserId = @UserId";

            return await conn.QueryAsync<CartItemViewModel>(
                sql, new { UserId = userId });
        }

        public async Task RemoveFromCartAsync(int cartId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            await conn.ExecuteAsync(
                "DELETE FROM Cart WHERE CartId = @CartId",
                new { CartId = cartId });
        }
    }
}
