
namespace myshop.BLL.ViewModels.Customer.Orders;

public class AddDeliveryInfoVM
{
    public int CartId { get; set; }
    public string ApplicationUserId { get; set; } = null!;
    
    public string Name { get; set; }


    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string City { get; set; } = null!;


    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string Address { get; set; } = null!;

    public int PaymentMethod { get; set; }

    public string? Notes { get; set; }


    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public string PhoneNumber { get; set; } = null!;
}
