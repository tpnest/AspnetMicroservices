using Dapper;
using Discount.Api.Entities;
using Npgsql;
using System.Data;

namespace Discount.Api.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDbConnection _conn;

        public DiscountRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            _conn = new NpgsqlConnection(connectionString);
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            try
            {
                var affected = await _conn.ExecuteAsync(
                    "INSERT INTO Coupon(ProductName,Description,Amount) VALUES(@ProductName,@Description,@Amount)",
                    new { coupon.ProductName, coupon.Description, coupon.Amount });

                return affected != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            try
            {
                var affected = await _conn.ExecuteAsync(
                    "DELETE FROM Coupon WHERE ProductName = @productName",
                    new { productName });

                return affected != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            try
            {
                var coupon = await _conn.QueryFirstOrDefaultAsync<Coupon>(
                    "SELECT Id, ProductName, Description, Amount FROM Coupon WHERE ProductName = @productName",
                    new { productName });

                return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Coupon { ProductName = "Error", Amount = 0, Description = "Database error" };
            }
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            try
            {
                var affected = await _conn.ExecuteAsync(
                    "UPDATE Coupon Set ProductName = @ProductName,Description = @Description, Amount = @Amount WHERE Id = @Id",
                    new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

                return affected != 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}