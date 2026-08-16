
using myshop.BLL.DTOs.Admin;
using myshop.BLL.DTOs.General;

namespace myshop.BLL.IServices.Admin;

public interface IAdminOrderService
{
    Task<PagingDTO<OrderDto>> GetAllOrdersAsync(FormDto model, CancellationToken ct);

}
