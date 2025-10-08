using Carter;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Dtos.ProductDtos;
using IztekCafe.WebApi.Extensions;
using IztekCafe.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IztekCafe.WebApi.Endpoints
{
    public class ProductEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/products")
                .WithTags("Products")
                .WithOpenApi();

            group.MapGet("/pageNumber/{pageNumber:int}/pageSize/{pageSize:int}", async ([FromServices] IProductService productService, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
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
                var result = await productService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllProductsPaged")
               .WithSummary("Tüm ürünleri sayfalandırarak getirir");

            group.MapGet("/", async ([FromServices] IProductService productService, CancellationToken cancellationToken) =>
            {
                var result = await productService.GetAsync(cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllProducts")
               .WithSummary("Tüm ürünleri getirir");
            group.MapGet("/actives", async ([FromServices] IProductService productService, CancellationToken cancellationToken) =>
            {
                var result = await productService.GetActivesAsync(cancellationToken);
                return result.ToGenericResult();
            })
         .WithName("GetAllActiveProducts")
         .WithSummary("Tüm aktif ürünleri getirir");

            group.MapGet("/{id:int}", async (int id, [FromServices] IProductService productService, CancellationToken cancellationToken) =>
            {
                var result = await productService.GetByIdAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
                .WithName("GetProductById")
                .WithSummary("Belirli bir ürünü getirir");

            group.MapPost("/", async ([FromBody] CreateProductDto dto, [FromServices] IProductService productService, CancellationToken cancellationToken) =>
            {
                var result = await productService.CreateAsync(dto, cancellationToken);
                return result.ToGenericResult();
            })
            .WithName("CreateProduct")
            .WithSummary("Yeni bir ürün oluşturur")
            .AddEndpointFilter<ValidationFilter<CreateProductDto>>();

            group.MapPut("/{id:int}", async (int id, [FromBody] UpdateProductDto dto, [FromServices] IProductService productService, CancellationToken cancellationToken) =>
            {
                var result = await productService.UpdateAsync(id, dto, cancellationToken);
                return result.ToResult();
            })
             .WithName("UpdateProduct")
             .WithSummary("Bir ürünü günceller")
             .AddEndpointFilter<ValidationFilter<UpdateProductDto>>();

            group.MapDelete("/{id:int}", async (int id, [FromServices] IProductService productService, CancellationToken cancellationToken) =>
            {
                var result = await productService.DeleteAsync(id, cancellationToken);
                return result.ToResult();
            })
           .WithName("DeleteProduct")
           .WithSummary("Bir ürünü siler");
        }
    }
}