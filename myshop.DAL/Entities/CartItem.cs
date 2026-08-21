
namespace myshop.DAL.Entities;

public class CartItem : DomainModelBase
{
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }


    /// <summary>
    /// Navigation Property
    /// </summary>
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }


    public int CartId { get; set; }
    [ForeignKey(nameof(CartId))]
    public ShoppingCart ShoppingCart { get; set; }
}
