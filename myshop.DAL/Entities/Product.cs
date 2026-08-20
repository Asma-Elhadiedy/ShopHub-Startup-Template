
namespace myshop.DAL.Entities;

public class Product : DomainModelBase
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public decimal Price { get; set; }


    /// <summary>
    /// Navigation Property
    /// </summary>
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    /// <summary>
    /// Navigation Collection
    /// </summary>
    public ICollection<CartItem>? CartItems { get; set; } = [];
    public ICollection<Review>? Reviews { get; set; } = [];
}
