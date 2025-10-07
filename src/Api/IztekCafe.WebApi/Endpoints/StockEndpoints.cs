using Carter;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Dtos.StockDtos;
using IztekCafe.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace IztekCafe.WebApi.Endpoints
{
    public class StockEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/stocks")
                .WithTags("Stocks")
                .WithOpenApi();

            group.MapGet("/pageNumber/{pageNumber:int}/pageSize/{pageSize:int}", async ([FromServices] IStockService stockService, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
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
                var result = await stockService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllStocksPaged")
               .WithSummary("Tüm stokları sayfalandırarak getirir");

            group.MapGet("/", async ([FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.GetAsync(cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllStocks")
               .WithSummary("Tüm stokları ürünleriyle birlikte getirir");

            group.MapGet("/{id:int}", async (int id, [FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.GetByIdAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
                .WithName("GetStockById")
                .WithSummary("Belirli bir stoğu ürünüyle birlikte getirir");

            group.MapPost("/", async ([FromBody] CreateStockDto dto, [FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.CreateAsync(dto, cancellationToken);
                return result.ToGenericResult();
            })
            .WithName("CreateStock")
            .WithSummary("Bir ürün için yeni bir stok oluşturur");

            group.MapPut("/{id:int}", async (int id, [FromBody] UpdateStockDto dto, [FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.UpdateAsync(id, dto, cancellationToken);
                return result.ToResult();
            })
             .WithName("UpdateStock")
             .WithSummary("Bir stoğu günceller");

            group.MapDelete("/{id:int}", async (int id, [FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.DeleteAsync(id, cancellationToken);
                return result.ToResult();
            })
           .WithName("DeleteStock")
           .WithSummary("Bir stoğu siler");

            group.MapPut("/{productId:int}/decrease/{quantity:int}", async (int productId, int quantity, [FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.DecreaseAsync(productId, quantity, cancellationToken);
                return result.ToGenericResult();
            })
          .WithName("DecreaseStock")
          .WithSummary("Bir stoğun miktarını azaltır");

            group.MapPut("/{productId:int}/increase/{quantity:int}", async (int productId, int quantity, [FromServices] IStockService stockService, CancellationToken cancellationToken) =>
            {
                var result = await stockService.IncreaseAsync(productId, quantity, cancellationToken);
                return result.ToGenericResult();
            })
        .WithName("IncreaseStock")
        .WithSummary("Bir stoğun miktarını artırır");
        }
    }
}