
using myshop.BLL.DTOs.General;
using myshop.BLL.ViewModels.Admin.Orders;

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

    public async Task<OrderDetailsVM?> GetOrderDetailsAsync(int orderId, CancellationToken ct)
    {
        try
        {
            var order = await _unitOfWork.Repository<OrderHeader>()
                .GetItemAsync(order => order.Id == orderId, ct, order => order.OrderItems);

            if (order is null)
                return null;

            return new OrderDetailsVM
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate,
                DeliveryDate = order.DeliveryDate,
                PaymentDate = order.PaymentDate,
                OrderStatus = order.OrderStatus.ToString(),
                PaymentStatus = order.PaymentStatus.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),
                Subtotal = order.Subtotal,
                ShippingCost = order.ShippingCost,
                TotalPrice = order.TotalPrice,
                TrakcingNumber = order.TrakcingNumber,
                Carrier = order.Carrier,
                CustomerName = order.CustomerName,
                EmailAddres = order.EmailAddres,
                Address = order.Address,
                City = order.City,
                PhoneNumber = order.PhoneNumber,
                AdditionalNotes = order.AdditionalNotes,
                Items = order.OrderItems?.Select(item => new OrderDetailsItemVM
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList() ?? []
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve order details for order {OrderId}.", orderId);
            throw;
        }
    }
}
