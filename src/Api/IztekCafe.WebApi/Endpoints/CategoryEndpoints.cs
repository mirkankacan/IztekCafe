using Carter;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Dtos.CategoryDtos;
using IztekCafe.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace IztekCafe.WebApi.Endpoints
{
    public class CategoryEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/categories")
                .WithTags("Categories")
                .WithOpenApi();

            group.MapGet("/pageNumber/{pageNumber:int}/pageSize/{pageSize:int}", async ([FromServices] ICategoryService categoryService, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
            {
                if (pageNumber <= 0) pageNumber = 1;
                switch (pageSize)
                {
                    case <= 10:
                        pageSize = 10;
                        break;

                    case >= 100:
                        pageSize = 100;
                        break;
                }
                var result = await categoryService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllCategoriesPaged")
               .WithSummary("Tüm kategorileri sayfalandırarak getirir");

            group.MapGet("/", async ([FromServices] ICategoryService categoryService, CancellationToken cancellationToken) =>
            {
                var result = await categoryService.GetAsync(cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllCategories")
               .WithSummary("Tüm kategorileri getirir");

            group.MapGet("/{id:int}", async (int id, [FromServices] ICategoryService categoryService, CancellationToken cancellationToken) =>
            {
                var result = await categoryService.GetByIdAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
                .WithName("GetCategoryByIdWithProducts")
                .WithSummary("Belirli bir kategoriyi içerdiği ürünlerle birlikte getirir");

            group.MapPost("/", async ([FromBody] CreateCategoryDto dto, [FromServices] ICategoryService categoryService, CancellationToken cancellationToken) =>
            {
                var result = await categoryService.CreateAsync(dto, cancellationToken);
                return result.ToGenericResult();
            })
            .WithName("CreateCategory")
            .WithSummary("Yeni bir kategori oluşturur");

            group.MapPut("/{id:int}", async (int id, [FromBody] UpdateCategoryDto dto, [FromServices] ICategoryService categoryService, CancellationToken cancellationToken) =>
            {
                var result = await categoryService.UpdateAsync(id, dto, cancellationToken);
                return result.ToGenericResult();
            })
             .WithName("UpdateCategory")
             .WithSummary("Bir kategoriyi günceller");

            group.MapDelete("/{id:int}", async (int id, [FromServices] ICategoryService categoryService, CancellationToken cancellationToken) =>
            {
                var result = await categoryService.DeleteAsync(id, cancellationToken);
                return result.ToResult();
            })
             .WithName("DeleteCategory")
             .WithSummary("Bir kategoriyi siler");
        }
    }
}