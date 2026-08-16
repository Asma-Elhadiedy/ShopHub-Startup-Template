

namespace myshop.BLL.DTOs.Customer;

public class CustomerOrderDto
{
    public string ApplicationUserId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string OrderStatus { get; set; } = null!;

}
