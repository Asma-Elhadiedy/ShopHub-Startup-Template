

using myshop.Application.DTOs;
using myshop.Domain.Entities;

namespace myshop.Application.Mappings;

public class MappingProfie : Profile
{
    public MappingProfie()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();

    }

}
