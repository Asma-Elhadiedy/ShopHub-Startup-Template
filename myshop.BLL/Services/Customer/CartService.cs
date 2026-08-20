
namespace myshop.BLL.Services.Customer;

public class CartService(
    ILogger<CartService> _logger,
    IUnitOfWork _unitOfWork) : ICartService
{
    public async Task<int> GetCartItemsCountAsync(int shoppingCartId, string? sessionId, string? userId, CancellationToken ct)
    {
        try
        {
            Expression<Func<CartItem, bool>> predicateCartItems = shoppingCartId > 0
                ? ci => ci.ShoppingCartId == shoppingCartId
                : userId is not null
                    ? ci => ci.ShoppingCart.ApplicationUserId == userId && ci.ShoppingCart.Status == eCartStatus.Active
                    : ci => ci.ShoppingCart.SessionId == sessionId && ci.ShoppingCart.Status == eCartStatus.Active;

            return await _unitOfWork.Repository<CartItem>()
                .SumAsync(predicateCartItems, ci => ci.Quantity, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<CartVM> GetCartByIdAsync(int shoppingCartId, string? userId, CancellationToken ct)
    {
        try
        {
            var cartContent = await _unitOfWork.Repository<CartItem>()
                .GetAllSelectedAsync(
                    ci => ci.ShoppingCartId == shoppingCartId,
                    ci => new CartItemVM
                    {
                        Id = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.UnitPrice
                    }, ct);

            return new()
            {
                Id = shoppingCartId,
                UserId = userId,
                Items = cartContent
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    public async Task<CartVM> GetCartDataAsync(string? userId, string? sessionId, CancellationToken ct)
    {
        try
        {
            //Expression<Func<ShoppingCart, bool>> predicateExistingCart =
            //    sc => sc.Status == eCartStatus.Active
            //        && ((!string.IsNullOrEmpty(sc.ApplicationUserId) && sc.ApplicationUserId == userId) 
            //        || sc.SessionId == sessionId 
            //        && sc.Status == eCartStatus.Active);

            Expression<Func<ShoppingCart, bool>> predicateExistingCart =
                !string.IsNullOrEmpty(userId)
                   ? sc => sc.ApplicationUserId == userId && sc.Status == eCartStatus.Active
                   : sc => sc.SessionId == sessionId && sc.Status == eCartStatus.Active;

            var shoppingCartId = await _unitOfWork.Repository<ShoppingCart>()
                .GetItemSelectedAsync(predicateExistingCart, c => c.Id, ct);

            var cartContent = shoppingCartId == 0
                ? []
                : await _unitOfWork.Repository<CartItem>()
                    .GetAllSelectedAsync(
                        ci => ci.ShoppingCartId == shoppingCartId,
                        ci => new CartItemVM
                        {
                            Id = ci.Id,
                            ProductId = ci.ProductId,
                            ProductName = ci.Product.Name,
                            Quantity = ci.Quantity,
                            UnitPrice = ci.UnitPrice
                        }, ct);

            return new()
            {
                Id = shoppingCartId,
                //SessionId = sessionId,
                Items = cartContent,
                UserId = userId,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> AddUpdateCartAsync(AddCartItemVM model, CancellationToken ct)
    {
        try
        {
            bool updatingCartResult;
            Expression<Func<ShoppingCart, bool>> predicateExistingCart = model.ShoppingCartId != 0
                ? p => p.Status == eCartStatus.Active && p.Id == model.ShoppingCartId
                : model.UserId is not null
                    ? p => p.Status == eCartStatus.Active && p.ApplicationUserId == model.UserId
                    : p => p.Status == eCartStatus.Active && p.SessionId == model.SessionId;

            var cartId = await _unitOfWork.Repository<ShoppingCart>()
                .GetItemSelectedAsync(predicateExistingCart, sc => sc.Id, ct);


            var transactionResult = await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                if (cartId == 0)
                {
                    var productPrice = await _unitOfWork.Repository<Product>()
                        .GetItemSelectedAsync(
                            p => p.Id == model.ProductId,
                            p => p.Price, ct);

                    ShoppingCart newCart = new()
                    {
                        SessionId = model.SessionId,
                        ApplicationUserId = model.UserId,
                        CartItems = [new()
                    {
                        Quantity = 1,
                        UnitPrice = productPrice,
                        ProductId = model.ProductId,
                        ShoppingCartId = cartId
                    }]
                    };

                    _unitOfWork.Repository<ShoppingCart>().Add(newCart);
                    updatingCartResult = await _unitOfWork.CompleteAsync(ct) > 0;
                    cartId = newCart.Id;
                }
                else
                {
                    model.ShoppingCartId = cartId;
                    updatingCartResult = await AddUpdateCartItemQuantityAsync(model, ct);
                }

                return updatingCartResult;
            }, ct);

            return transactionResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }
    public async Task<bool> AddUpdateCartItemQuantityAsync(AddCartItemVM model, CancellationToken ct)
    {
        try
        {
            Expression<Func<CartItem, bool>> predicateExistingItem = model.Id != 0
                ? ci => ci.Id == model.Id
                : ci => ci.ShoppingCart.Status == eCartStatus.Active
                    && ci.ShoppingCartId == model.ShoppingCartId
                    && ci.ProductId == model.ProductId;

            var existingCartItem = await _unitOfWork.Repository<CartItem>().GetItemAsync(predicateExistingItem, ct);

            if (model.UserId is not null)
            {
                var applicationUserId = await _unitOfWork.Repository<ShoppingCart>()
                    .GetItemSelectedAsync(sc => sc.Id == model.ShoppingCartId, sc => sc.ApplicationUserId, ct);

                //If the cart is not associated with a user,
                //associate it with the provided userId and invalidate any old active carts for that user
                if (applicationUserId is null)
                {
                    var existingCart = await _unitOfWork.Repository<ShoppingCart>()
                        .GetItemAsync(sc => sc.Id == model.ShoppingCartId, ct);

                    if (existingCart is null)
                    {
                        _logger.LogError("Failed to get cart with id: {id}", model.ShoppingCartId);
                        return false;
                    }
                    existingCart.ApplicationUserId = model.UserId;
                    await SoftDeleteActiveCarts(model.UserId);
                }
            }

            if (existingCartItem is null)
            {
                var productPrice = await _unitOfWork.Repository<Product>()
                .GetItemSelectedAsync(
                    p => p.Id == model.ProductId,
                    p => p.Price, ct);

                CartItem newCartItem = new()
                {
                    Quantity = 1,
                    UnitPrice = productPrice,
                    ProductId = model.ProductId,
                    ShoppingCartId = model.ShoppingCartId
                };

                _unitOfWork.Repository<CartItem>().Add(newCartItem);
            }
            else
                existingCartItem.Quantity = model.Quantity == 0
                    ? existingCartItem.Quantity + 1
                    : model.Quantity;

            if (await _unitOfWork.CompleteAsync(ct) > 0)
                return true;

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    public async Task<bool> RemoveCartItemAsync(int cartItemId, CancellationToken ct)
    {
        try
        {
            var cartItem = await _unitOfWork.Repository<CartItem>()
                .GetItemAsync(ci => ci.Id == cartItemId, ct);

            if (cartItem is null)
            {
                _logger.LogError("Failed to get cart item with id: {id}", cartItemId);
                return false;
            }

            _unitOfWork.Repository<CartItem>().Remove(cartItem);
            if (await _unitOfWork.CompleteAsync(ct) > 0)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }
    public async Task<bool> ClearCartAsync(int shoppingCartId, CancellationToken ct)
    {
        try
        {
            var cart = await _unitOfWork.Repository<ShoppingCart>()
                .GetItemAsync(ci => ci.Id == shoppingCartId, ct);

            if (cart is null)
            {
                _logger.LogError("Failed to get cart with id: {id}", shoppingCartId);
                return false;
            }

            cart.IsDeleted = true;
            cart.DeletedAt = DateTime.UtcNow;
            cart.Status = eCartStatus.Removed;
            //_unitOfWork.Repository<ShoppingCart>().Remove(cart);
            if (await _unitOfWork.CompleteAsync(ct) > 0)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }


    private async Task<int> SoftDeleteActiveCarts(string userId)
    {
        return await _unitOfWork.Repository<ShoppingCart>().BulkUpdateAsync(
            sc => sc.ApplicationUserId == userId && sc.Status == eCartStatus.Active,
            setters => setters.SetProperty(sc => sc.Status, eCartStatus.Removed)
                            .SetProperty(sc => sc.IsDeleted, true)
                            .SetProperty(sc => sc.DeletedAt, DateTime.UtcNow));
    }


}
