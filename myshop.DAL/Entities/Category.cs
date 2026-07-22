
namespace myshop.DAL.Entities;

public class Category : IDomainModelMarker
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;

}
