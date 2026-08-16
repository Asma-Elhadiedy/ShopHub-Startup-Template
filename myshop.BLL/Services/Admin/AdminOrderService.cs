
using myshop.BLL.DTOs.General;

namespace myshop.BLL.Services.Admin;

public class AdminOrderService(
    ILogger<OrderService> _logger,
    IUnitOfWork _unitOfWork,
    IMapper _mapper) : IAdminOrderService
{
    public async Task<PagingDTO<OrderDto>> GetAllOrdersAsync(FormDto model, CancellationToken ct)
    {
        try
        {
            var ordersQuery = _unitOfWork.Repository<OrderHeader>()
                .GetQueryable(null)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);

            if (model.SortingCol is not null && model.SortingDir is not null)
                ordersQuery = ordersQuery.OrderBy($"{model.SortingCol} {model.SortingDir}");

            var recordsTotal = ordersQuery.Count();
            var pagedOrders = await ordersQuery
                .Skip(model.Start)
                .Take(model.PageSize)
                .ToListAsync(ct);

            return new()
            {
                Data = pagedOrders,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsTotal,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving orders.");
            throw;
        }
    }
}
