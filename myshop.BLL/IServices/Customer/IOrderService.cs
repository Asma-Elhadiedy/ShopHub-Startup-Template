
namespace myshop.BLL.IServices.Customer;

public interface IOrderService
{
    Task<PagingDTO<CustomerOrderDto>> GetAllOrdersAsync(FormDto model, string userId, CancellationToken ct);

    Task<AddDeliveryInfoVM> PrepareDeliveryInfoModelAsync(string userId, CancellationToken ct);

    Task<(int orderId, long total)> CreateOrderFromCartAsync(AddDeliveryInfoVM deliveryInfo, string userId, string email, CancellationToken ct);
    
    Task<bool> UpdateOrderPaymentStatusAsync(int orderId, ePaymentStatus estatus, CancellationToken ct);
}
