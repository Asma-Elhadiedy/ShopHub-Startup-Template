
namespace myshop.DAL.Entities;

public class OrderItem : DomainModelBase
{
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }


    /// <summary>
    /// Navigation Property
    /// </summary>
    public int OrderId { get; set; }
    [ForeignKey(nameof(OrderId))]
    public OrderHeader? Order { get; set; } 

    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

}
