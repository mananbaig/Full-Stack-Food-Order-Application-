using Dapper;
using FoodOrder.Models.Interfaces;
using Microsoft.Data.SqlClient;

namespace FoodOrder.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodOrder;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT UserName, Email, Phone, Address FROM dbo.AspNetUsers WHERE Email = @Email";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
            if (user == null) throw new Exception("User not found");
            return user;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT UserName, Email, Phone, Address FROM dbo.AspNetUsers WHERE Id = @UserId";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });
            if (user == null) throw new Exception("User not found");
            return user;
        }

        public async Task<int> GetTotalOrdersAsync(string userId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT COUNT(*) FROM dbo.Orders WHERE UserId = @UserId";
            return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }

        public async Task<decimal> GetTotalSpentAsync(string userId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT ISNULL(SUM(TotalAmount), 0) FROM dbo.Orders WHERE UserId = @UserId";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { UserId = userId });
        }

        public async Task<int> GetTotalItemsOrderedAsync(string userId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT ISNULL(SUM(oi.Quantity), 0) FROM dbo.OrderItem oi INNER JOIN dbo.Orders o ON oi.OrderId = o.OrderId WHERE o.UserId = @UserId";
            return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }

        public async Task<int> GetPendingOrdersAsync(string userId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT COUNT(*) FROM dbo.Orders WHERE UserId = @UserId AND Status = 'Pending'";
            return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }
    }
}
