
namespace myshop.BLL.ViewModels.Customer.Products;

public class ShoppingProductVM
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string? ImagePath { get; set; }
    public decimal Price { get; set; }

}
