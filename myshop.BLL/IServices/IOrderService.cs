namespace myshop.BLL.IServices;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

}
