
namespace myshop.DAL.Entities;

public class ApplicationSetting : IDomainModelMarker
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
} 
