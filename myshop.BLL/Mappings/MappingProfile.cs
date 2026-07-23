
namespace myshop.BLL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductVM>().ReverseMap();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryVM>().ReverseMap();

        CreateMap<UserDto, ApplicationUser>().ReverseMap();
        CreateMap<ApplicationUser, RegisterVM>().ReverseMap();
        CreateMap<ApplicationUser, UserVM>().ReverseMap();
        
        CreateMap<OrderHeader, OrderDto>().ReverseMap();

        //CreateMap<Product, ProductDto>()
        //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

    }


}
