
using System.Net;
using System.Text;

namespace myshop.BLL.Services.Customer;

public class OrderService(
    ILogger<OrderService> _logger,
    IUnitOfWork _unitOfWork,
    IEmailSenderService _emailSender,
    DomainPathService _domainPath,
    IMapper _mapper) : IOrderService
{
    public async Task<PagingDTO<CustomerOrderDto>> GetAllOrdersAsync(FormDto model, string userId, CancellationToken ct)
    {
        try
        {
            var ordersQuery = _unitOfWork.Repository<OrderHeader>()
                .GetQueryable(null)
                .ProjectTo<CustomerOrderDto>(_mapper.ConfigurationProvider);

            ordersQuery = ordersQuery.Where(o => o.ApplicationUserId == userId);

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

    public async Task<AddDeliveryInfoVM> PrepareDeliveryInfoModelAsync(string userId, CancellationToken ct)
    {
        try
        {
            var activeCartModel = await _unitOfWork.Repository<ShoppingCart>()
                .GetItemSelectedAsync(
                    sc => sc.ApplicationUserId == userId && sc.Status == eCartStatus.Active,
                    sc => new AddDeliveryInfoVM
                    {
                        CartId = sc.Id
                    }
                    , ct);

            return activeCartModel is null ? new() : activeCartModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<(int orderId, long total)> CreateOrderFromCartAsync(AddDeliveryInfoVM deliveryInfo, string userId, string email, CancellationToken ct)
    {
        try
        {
            var updateUserDeliveryInfo = await AddDeliveryInfoToUserAsync(deliveryInfo, ct);
            if (!updateUserDeliveryInfo)
                return (0, 0);

            var cart = await _unitOfWork.Repository<ShoppingCart>()
                .GetItemAsync(sc =>
                    sc.Id == deliveryInfo.CartId
                    //&& sc.ApplicationUserId == userId
                    && sc.Status == eCartStatus.Active
                    , ct
                    , sc => sc.CartItems);

            if (cart is null)
            {
                _logger.LogError("Failed to get cart with id: {id}, for user: {userId}", deliveryInfo.CartId, userId);
                return (0, 0);
            }

            var productIds = cart.CartItems?.Select(c => c.ProductId) ?? [];
            var products = await _unitOfWork.Repository<Product>()
                .GetAllSelectedAsync(
                    p => productIds.Contains(p.Id),
                    p => new KeyValuePair<int, string>(p.Id, p.Name)
                    , ct);

            var productsDict = new Dictionary<int, string>(products);

            cart.ApplicationUserId ??= userId;
            cart.Status = eCartStatus.CheckedOut;
            var order = cart.MapCartToOrder(deliveryInfo, productsDict, email);
            _unitOfWork.Repository<OrderHeader>().Add(order);

            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                var isEmailSent = await SendOrderConfirmationEmailAsync(order, ct);
                return (order.Id, (long)order.OrderItems?.Sum(i => i.Quantity * i.UnitPrice)!);
            }
            return (0, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateOrderPaymentStatusAsync(int orderId, ePaymentStatus eStatus, CancellationToken ct)
    {
        try
        {
            var order = await _unitOfWork.Repository<OrderHeader>().GetItemAsync(o => o.Id == orderId, ct, o => o.OrderItems);
            if (order is null)
            {
                _logger.LogWarning("Can't find order with ID: {orderId}", orderId);
                return false;
            }

            order.OrderStatus = eOrderStatus.Confirmed;
            order.PaymentStatus = eStatus;
            if (await _unitOfWork.CompleteAsync(ct) > 0)
            {
                var isEmailSent = await SendOrderConfirmationEmailAsync(order, ct);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }


    async Task<bool> AddDeliveryInfoToUserAsync(AddDeliveryInfoVM model, CancellationToken ct)
    {
        try
        {
            var existingPhoneNumber = await _unitOfWork.Repository<ApplicationUser>()
                .GetItemSelectedAsync(u => u.Id == model.ApplicationUserId, u => u.PhoneNumber, ct);

            if (existingPhoneNumber is not null)
                return true;

            var customer = await _unitOfWork.Repository<ApplicationUser>()
                .GetItemAsync(u => u.Id == model.ApplicationUserId, ct);

            if (customer is null)
            {
                _logger.LogWarning("Can't find user with ID: {userId}", model.ApplicationUserId);
                return false;
            }
            customer.Address = model.Address;
            customer.PhoneNumber = model.PhoneNumber;
            customer.City = model.City;

            return await _unitOfWork.CompleteAsync(ct) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    async Task<bool> SendOrderConfirmationEmailAsync(OrderHeader order, CancellationToken ct)
    {
        try
        {
            string subject = $"Your Order #{order.Id} from My Shop is Confirmed!";
            string toName = order.CustomerName;
            string toEmail = order.EmailAddres;
            string htmlTemplate = await File.ReadAllTextAsync(Path.Combine(ConstPath.EmailTemplatesPath, ConstPath.OrderConfirmEmailTemplatePath), ct);

            var itemsRowsHtml = BuildItemsRowsHtml(order.OrderItems);

            var emailBody = htmlTemplate
                .Replace("{{CustomerName}}", order.CustomerName)
                .Replace("{{OrderId}}", order.Id.ToString())
                .Replace("{{OrderDate}}", order.CreatedAt.ToString("MMM dd, yyyy"))
                .Replace("{{ItemsRows}}", itemsRowsHtml)
                .Replace("{{Subtotal}}", order.Subtotal.ToString("C"))
                .Replace("{{Shipping}}", order.ShippingCost > 0 ? order.ShippingCost.ToString("C") : "Free")
                .Replace("{{Total}}", order.TotalPrice.ToString("C"))
                .Replace("{{DeliveryAddress}}", order.Address)
                .Replace("{{DeliveryCity}}", order.City)
                .Replace("{{DeliveryPhone}}", order.PhoneNumber)
                .Replace("{{PaymentMethod}}", order.PaymentMethod.ToString())
                .Replace("{{OrderTrackingUrl}}", _domainPath.GetDomainPath())
                .Replace("{{CurrentYear}}", DateTime.UtcNow.Year.ToString());

            return await _emailSender.SendAsync(new SendEmailDto(toName, toEmail, subject, emailBody), ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }
    static string BuildItemsRowsHtml(IEnumerable<OrderItem> items)
    {
        var sb = new StringBuilder();

        foreach (var item in items)
        {
            sb.Append($"""
            <tr>
                <td style="font-size:14px; color:#1a1a2e; padding:10px 0; border-bottom:1px solid #eef1f6;">{WebUtility.HtmlEncode(item.ProductName)}</td>
                <td align="center" style="font-size:14px; color:#556; padding:10px 0; border-bottom:1px solid #eef1f6;">{item.Quantity}</td>
                <td align="right" style="font-size:14px; color:#1a1a2e; padding:10px 0; border-bottom:1px solid #eef1f6;">{(item.UnitPrice * item.Quantity).ToString("C")}</td>
            </tr>
            """);
        }

        return sb.ToString();
    }

}