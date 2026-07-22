
namespace myshop.BLL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryVM>().ReverseMap();
        CreateMap<Product, ProductVM>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationUser, RegisterVM>().ReverseMap();

        //CreateMap<Product, ProductDto>()
        //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

    }


}
