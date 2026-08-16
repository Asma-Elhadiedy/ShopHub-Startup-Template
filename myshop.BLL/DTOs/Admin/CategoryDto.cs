namespace myshop.BLL.DTOs.Admin;

public class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
 
    public string Description { get; set; } = null!;
    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
