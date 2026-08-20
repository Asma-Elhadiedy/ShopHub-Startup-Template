
using myshop.BLL.DTOs.Admin;
using myshop.BLL.DTOs.General;
using myshop.BLL.ViewModels.Admin.Orders;

namespace myshop.BLL.IServices.Admin;

public interface IAdminOrderService
{
    Task<PagingDTO<OrderDto>> GetAllOrdersAsync(FormDto model, CancellationToken ct);
    Task<OrderDetailsVM?> GetOrderDetailsAsync(int orderId, CancellationToken ct);

}
