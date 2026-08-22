
namespace myshop.DAL.Entities;

public class OrderHeader : DomainModelBase
{
    public eOrderStatus OrderStatus { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public DateTime? PaymentDate { get; set; }
    public ePaymentStatus PaymentStatus { get; set; }
    public ePaymentMethod PaymentMethod { get; set; }

    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalPrice { get; set; }

    public string? TrakcingNumber { get; set; }
    public string? Carrier { get; set; }


    //Stripe Properties
    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }

    //User Data
    public string CustomerName { get; set; }
    public string EmailAddres { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AdditionalNotes { get; set; }

    /// <summary>
    /// Navigation Propert
    /// </summary>
    public string ApplicationUserId { get; set; } = null!;
    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }


    public int CartId { get; set; }
    [ForeignKey(nameof(CartId))]
    public ShoppingCart? ShoppingCart { get; set; }

    /// <summary>
    /// Navigation Collection
    /// </summary>
    public ICollection<OrderItem>? OrderItems { get; set; } = [];
}
