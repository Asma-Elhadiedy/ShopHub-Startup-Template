namespace myshop.BLL.ViewModels.Admin.Categories;

public class CategoryVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [StringLength(150, MinimumLength = 5, ErrorMessage = ConstMessages.StringLength)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = ConstMessages.RequiredInput)]
    [StringLength(150, MinimumLength = 5, ErrorMessage = ConstMessages.StringLength)]
    public string Description { get; set; } = null!;

    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
