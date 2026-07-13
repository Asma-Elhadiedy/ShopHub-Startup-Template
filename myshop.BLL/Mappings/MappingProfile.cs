


namespace myshop.BLL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryVM>().ReverseMap();
        CreateMap<Product, ProductVM>().ReverseMap();
    }


}
