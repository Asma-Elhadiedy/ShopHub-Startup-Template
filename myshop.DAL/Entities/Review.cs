namespace myshop.DAL.Entities;

public class Review : DomainModelBase
{
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;

    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser? ApplicationUser { get; set; }
}
