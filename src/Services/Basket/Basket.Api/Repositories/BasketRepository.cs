﻿using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task DeleteBasketAsync(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }

    public async Task<ShoppingCart?> GetBasketAsync(string userName)
    {
       var basket = await _redisCache.GetStringAsync(userName);

       return string.IsNullOrWhiteSpace(basket) ? null : JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart basket)
    {
        await _redisCache.SetStringAsync(basket.UserName,JsonSerializer.Serialize(basket));

        return await GetBasketAsync(basket.UserName);
    }
}
