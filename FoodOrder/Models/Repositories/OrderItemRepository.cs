
using Microsoft.Data.SqlClient;
using Dapper;
using FoodOrder.Models.Interfaces;

namespace FoodOrder.Models.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodOrder;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public async Task AddOrderItemAsync(IEnumerable<OrderItem> items)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "INSERT INTO OrderItem (OrderId, FoodId, Quantity, UnitPrice) VALUES (@OrderId, @FoodId, @Quantity, @UnitPrice)";
            await conn.ExecuteAsync(sql, items);
        }
        public async Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var items = await conn.QueryAsync<OrderItem>("SELECT OrderItemId, OrderId, FoodId, Quantity, UnitPrice FROM OrderItem WHERE OrderId = @OrderId", new { OrderId = orderId });
            return items;
        }
    }
}
