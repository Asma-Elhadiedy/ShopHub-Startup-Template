
namespace myshop.BLL.IServices.Customer;

public interface ICartService
{
    Task<int> GetCartItemsCountAsync(int shoppingCartId, string? sessionId, string? userId, CancellationToken ct);
    Task<CartVM> GetCartByIdAsync(int shoppingCartId, string? userId, CancellationToken ct);
    Task<CartVM> GetCartDataAsync(string? userId, string? sessionId, CancellationToken ct);
    Task<bool> AddUpdateCartAsync(AddCartItemVM model, CancellationToken ct);
    Task<bool> AddUpdateCartItemQuantityAsync(AddCartItemVM model, CancellationToken ct);
    Task<bool> RemoveCartItemAsync(int cartItemId, CancellationToken ct);
    Task<bool> ClearCartAsync(int shoppingCartId, CancellationToken ct);
    Task<int> SoftDeleteOldActiveCarts(string userId, CancellationToken ct, int cartId = 0);
    Task UpdateCartOwnershipAsync(int shoppingCartId, string userId, CancellationToken ct);
}
