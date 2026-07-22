
namespace myshop.BLL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductVM>().ReverseMap();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryVM>().ReverseMap();

        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationUser, RegisterVM>().ReverseMap();
        
        CreateMap<OrderHeader, OrderDto>().ReverseMap();

        //CreateMap<Product, ProductDto>()
        //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

    }


}
