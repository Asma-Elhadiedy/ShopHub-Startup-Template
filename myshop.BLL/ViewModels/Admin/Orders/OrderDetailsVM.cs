namespace myshop.BLL.ViewModels.Admin.Orders;

public class OrderDetailsVM
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalPrice { get; set; }
    public string? TrakcingNumber { get; set; }
    public string? Carrier { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string EmailAddres { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? AdditionalNotes { get; set; }
    public List<OrderDetailsItemVM> Items { get; set; } = [];
}

public class OrderDetailsItemVM
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}
