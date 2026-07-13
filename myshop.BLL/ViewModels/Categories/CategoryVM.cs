
namespace myshop.BLL.ViewModels.Categories;

public class CategoryVM
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
