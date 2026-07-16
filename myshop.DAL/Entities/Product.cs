
namespace myshop.DAL.Entities;

public class Product : IDomainModelMarker
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Description { get; set; }

    [DisplayName("Image")]
    [ValidateNever]
    public string Img { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }
    [ValidateNever]
    public Category Category { get; set; }
}
