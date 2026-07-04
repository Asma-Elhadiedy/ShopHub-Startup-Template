namespace myshop.DAL.Entities;

public class ShoppingCart
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    [ValidateNever]
    public Product Product { get; set; }

    public int Count { get; set; }

    public string ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
