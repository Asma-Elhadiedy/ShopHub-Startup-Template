
namespace myshop.BLL.ViewModels.Customer.Cart;

public class AddCartItemVM
{
    public int Id { get; set; }
    public string? SessionId { get; set; }
    public string? UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int ShoppingCartId { get; set; }
}
