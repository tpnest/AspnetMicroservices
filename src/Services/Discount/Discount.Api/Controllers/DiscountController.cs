using Discount.Api.Entities;
using Discount.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository discountRepository;

    public DiscountController(IDiscountRepository discountRepository)
    {
        this.discountRepository = discountRepository;
    }

    [HttpGet("{productName}")]
    public async Task<ActionResult<Coupon>> GetDiscount(string productName)
    {
        return Ok(await discountRepository.GetDiscount(productName));
    }

    [HttpPost]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
    {
        await discountRepository.CreateDiscount(coupon);
        return CreatedAtAction(nameof(GetDiscount), new { productName = coupon.ProductName }, coupon);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateDiscount([FromBody] Coupon coupon)
    {
        return Ok(await discountRepository.UpdateDiscount(coupon));
    }

    [HttpDelete("{productName}")]
    public async Task<ActionResult> DeleteDiscount(string productName)
    {
        return Ok(await discountRepository.DeleteDiscount(productName));
    }
}
