using IztekCafe.Application.Dtos.CategoryDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Extensions;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class CategoryMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateCategoryDto, Category>()
                 .Ignore(dest => dest.Products);

            config.NewConfig<Category, CategoryDto>()
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());

            config.NewConfig<Category, CategoryDetailDto>()
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());
        }
    }
}