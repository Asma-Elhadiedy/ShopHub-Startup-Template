


namespace myshop.BLL.Mappings;

public class MappingProfie : Profile
{
    public MappingProfie()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryVM>().ReverseMap();
        CreateMap<Product, ProductVM>().ReverseMap();
    }


}
