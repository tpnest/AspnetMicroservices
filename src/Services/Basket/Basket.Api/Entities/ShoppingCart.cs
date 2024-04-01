namespace Basket.Api.Entities;

public class ShoppingCart
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
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
            return Items.Sum(item => item.Price * item.Quantity);
        }
    }
}
