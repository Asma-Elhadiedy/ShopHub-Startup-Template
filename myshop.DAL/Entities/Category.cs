
namespace myshop.DAL.Entities;

public class Category : DomainModelBase
{
    public string Name { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// Navigation Collection
    /// </summary>
    public ICollection<Product>? Products { get; set; } = [];
}
