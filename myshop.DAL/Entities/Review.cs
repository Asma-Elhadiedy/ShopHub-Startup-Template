namespace myshop.DAL.Entities;

public class Review : DomainModelBase
{
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;

    /// <summary>
    /// Navigation Property
    /// </summary>
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public string ApplicationUserId { get; set; } = null!;
    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }
}
