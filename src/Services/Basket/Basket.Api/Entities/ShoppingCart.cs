namespace Basket.Api.Entities;

public class ShoppingCart
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// 购物车商品
    /// </summary>
    public List<ShoppingCartItem> Items { get; set; } = new();

    public ShoppingCart() { }

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    /// <summary>
    /// 总价
    /// </summary>
    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (var item in Items)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}
