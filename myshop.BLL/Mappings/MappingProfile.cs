
using myshop.BLL.DTOs.Admin;
using myshop.BLL.ViewModels.Admin.Categories;
using myshop.BLL.ViewModels.Admin.Products;
using myshop.BLL.ViewModels.Customer.Cart;

namespace myshop.BLL.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductVM, Product>().ReverseMap()
            .ForMember(
                d => d.Img,
                o => o.MapFrom(s => string.IsNullOrEmpty(s.ImagePath) ? ConstPath.DefaultProductImagePath : s.ImagePath));

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryVM>().ReverseMap();

        CreateMap<UserDto, ApplicationUser>().ReverseMap();
        CreateMap<ApplicationUser, RegisterVM>().ReverseMap();
        CreateMap<ApplicationUser, UserVM>().ReverseMap();
        
        CreateMap<OrderHeader, OrderDto>().ReverseMap();

        CreateMap<CartItem, CartItemVM>().ReverseMap();
        CreateMap<CartItem, AddCartItemVM>().ReverseMap();

        //CreateMap<Product, ProductDto>()
        //    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<OrderHeader, CustomerOrderDto>().ReverseMap();
        CreateMap<AddDeliveryInfoVM, ApplicationUser>().ReverseMap();

    }


}
