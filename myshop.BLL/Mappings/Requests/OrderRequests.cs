
namespace myshop.BLL.Mappings.Requests;

internal static class OrderRequests
{
    extension(ShoppingCart cart)
    {
        internal OrderHeader MapCartToOrder(AddDeliveryInfoVM deliveryInfo, Dictionary<int, string> productsDict, string email)
        {
            return new OrderHeader
            {
                ApplicationUserId = cart.ApplicationUserId!,
                OrderDate = DateTime.UtcNow,
                OrderStatus = deliveryInfo.PaymentMethod == (int)ePaymentMethod.CashOnDelivery
                    ? eOrderStatus.Confirmed
                    : eOrderStatus.WaitingForE_Payment,
                PaymentStatus = deliveryInfo.PaymentMethod == (int)ePaymentMethod.CashOnDelivery
                    ? ePaymentStatus.WaitingForCash
                    : ePaymentStatus.Pending,
                PaymentMethod = (ePaymentMethod)deliveryInfo.PaymentMethod,
                CartId = cart.Id,
                Subtotal = cart.CartItems!.Sum(ci => ci.Quantity * ci.UnitPrice),

                CustomerName = deliveryInfo.Name,
                EmailAddres = email,
                Address = deliveryInfo.Address,
                City = deliveryInfo.City,
                PhoneNumber = deliveryInfo.PhoneNumber,
                AdditionalNotes = deliveryInfo.Notes,

                OrderItems = [.. cart.CartItems!.Select(ci => new OrderItem
                {
                    ProductName = productsDict.GetValueOrDefault(ci.ProductId) ?? "",
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                })]
            };
        }
    }
}
