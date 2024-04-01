﻿using Dapper;
using Discount.Api.Entities;
using Npgsql;

namespace Discount.Api.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await conn.ExecuteAsync("INSERT INTO Coupon(ProductName,Description,Amount) VALUES(@ProductName,@Description,@Amount)",
            new { coupon.ProductName, coupon.Description, coupon.Amount });

        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await conn.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @productName",
                             new { productName });

        return affected != 0;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @productName",
                             new { productName });

        return coupon is not null
            ? coupon
            : new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected = await conn.ExecuteAsync("UPDATE Coupon Set ProductName = @ProductName,Description = @Description, Amount = @Amount WHERE Id = @Id",
            new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

        return affected != 0;
    }
}
