
using Microsoft.Data.SqlClient;
using Dapper;
using FoodOrder.Models.Interfaces;

namespace FoodOrder.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodOrder;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public async Task<int> CreateOrderAsync(Order order)
        {
            using var conn = new SqlConnection(_connectionString);

            string sql = @"
            INSERT INTO Orders (UserId, OrderDate, TotalAmount, Status)
            VALUES (@UserId, @OrderDate, @TotalAmount, @Status);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";
            int orderId = await conn.ExecuteScalarAsync<int>(sql, order);
            return orderId;
        }

        public async Task<int> GetCount()
        {
           var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "SELECT COUNT(*) FROM [Order]";
            var count = await conn.ExecuteScalarAsync<int>(sql);
            return count;

        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var items = await conn.QueryAsync<Order>(
                "SELECT OrderId, UserId, OrderDate, TotalAmount, Status FROM [Order] WHERE UserId = @UserId",
                new { UserId = userId }
            );
            return items;
        }
    }
}
