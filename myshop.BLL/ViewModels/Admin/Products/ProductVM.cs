namespace myshop.BLL.ViewModels.Admin.Products;

public class ProductVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [StringLength(150, MinimumLength = 5, ErrorMessage = ConstMessages.StringLength)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [StringLength(150, MinimumLength = 5, ErrorMessage = ConstMessages.StringLength)]
    public string Description { get; set; } = null!;


    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [Range(1.00, 1000000.00, ErrorMessage = ConstMessages.ValidRange)]
    public decimal Price { get; set; }

    [DisplayName("Category")]
    [Required(ErrorMessage = ConstMessages.RequiredSelect)]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    public IFormFile File { get; set; }
    public string? Img { get; set; }


    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; } = [];
}
