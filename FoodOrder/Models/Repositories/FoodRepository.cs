using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using Dapper;
using FoodOrder.Models.Interfaces;
namespace FoodOrder.Models.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FoodOrder;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public async Task AddAsync(FoodItem item)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "INSERT INTO FoodItem (Name, Description, Price, ImageUrl, isAvailable) VALUES (@Name, @Description, @Price, @ImageUrl, @isAvailable)";
            await conn.ExecuteAsync(sql, item);
        }
        public async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "SELECT FoodId , Name , Description , Price, ImageUrl , isAvailable  FROM FoodItem";
            var items = await conn.QueryAsync<FoodItem>(sql);
            return items;
        }

        public async Task<int> GetCount()
        {
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "SELECT COUNT(*) FROM FoodItem";
            var count = await conn.ExecuteScalarAsync<int>(sql);
            return count;
        }


        public async Task<FoodItem?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = "SELECT FoodId AS foodId, Name AS foodName, Description AS description, Price AS price, ImageUrl AS imageUrl, isAvailable AS isAvailable FROM FoodItem WHERE FoodId = @Id";
            var item = await conn.QueryFirstOrDefaultAsync<FoodItem>(sql, new { Id = id });
            if (item == null)
                return new FoodItem { Name = "ZZZZ", FoodId = 0000, Description = "NotFound", Price = 0000 };
            return item;
        }

        public async Task UpdateAsync(FoodItem item)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"UPDATE FoodItem 
                SET Name = @Name,
                    Description = @Description,
                    Price = @Price,
                    ImageUrl = @ImageUrl,
                    isAvailable = @isAvailable
                WHERE FoodId = @FoodId";

            await conn.ExecuteAsync(sql, item);
        }

        /* public async Task DeleteAsync(int id)
         {
             using var conn = new SqlConnection(_connectionString);
             await conn.OpenAsync();
             var sql = "DELETE FROM FoodItem WHERE FoodId = @Id";
             await conn.ExecuteAsync(sql, new { Id = id });
         }*/

        public async Task DeleteAsync(int foodId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(
                    "DELETE FROM dbo.Cart WHERE FoodId = @FoodId",
                    new { FoodId = foodId },
                    transaction
                );

                await connection.ExecuteAsync(
                    "DELETE FROM dbo.OrderItem WHERE FoodId = @FoodId",
                    new { FoodId = foodId },
                    transaction
                );

                await connection.ExecuteAsync(
                    "DELETE FROM dbo.FoodItem WHERE FoodId = @FoodId",
                    new { FoodId = foodId },
                    transaction
                );

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}

