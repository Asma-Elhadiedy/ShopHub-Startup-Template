namespace myshop.BLL.DTOs.Admin;

public class OrderDto
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string? TrakcingNumber { get; set; }


}
