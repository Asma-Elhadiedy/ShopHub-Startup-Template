
using System.ComponentModel;

namespace myshop.BLL.ViewModels.Products;

public class ProductVM
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string? Img { get; set; }

    [Required]
    public string Description { get; set; }

    
    [Required]
    public decimal Price { get; set; }

    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }


    public IFormFile File { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
