
using System.Runtime.CompilerServices;

namespace myshop.DAL.Entities;

public class ShoppingCart : DomainModelBase
{
    public string? SessionId { get; set; }
    public eCartStatus Status { get; set; } = eCartStatus.Active;

    /// <summary>
    /// Navigation Property
    /// </summary>
    public string? ApplicationUserId { get; set; }  
    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }

    /// <summary>
    /// Navigation Collection
    /// </summary>
    public ICollection<CartItem>? CartItems { get; set; } = [];
}
