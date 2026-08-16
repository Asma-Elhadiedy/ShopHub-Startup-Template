namespace myshop.BLL.ViewModels.Customer.Cart;

public class CartVM
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? SessionId { get; set; }
    public decimal TotalPrice => Items.Sum(i => i.TotalItemPrice);
    public int TotalItemsCount => Items.Sum(i => i.Quantity);
    public IEnumerable<CartItemVM> Items { get; set; }

}

public class CartItemVM
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal TotalItemPrice => UnitPrice * Quantity;

}
