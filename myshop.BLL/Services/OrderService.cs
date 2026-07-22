

namespace myshop.BLL.Services;

public class OrderService(ILogger<UserService> _logger, IUnitOfWork _unitOfWork, IMapper _mapper) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {

        return await _unitOfWork.Repository<OrderHeader>()
            .GetQueryable(null)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
