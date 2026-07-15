
namespace myshop.BLL.ViewModels.Categories;

public class CategoryVM
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
