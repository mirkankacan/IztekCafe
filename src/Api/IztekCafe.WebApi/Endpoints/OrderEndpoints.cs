using Carter;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Dtos.OrderDtos;
using IztekCafe.WebApi.Extensions;
using IztekCafe.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IztekCafe.WebApi.Endpoints
{
    public class OrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/orders")
                .WithTags("Orders")
                .WithOpenApi();

            group.MapGet("/pageNumber/{pageNumber:int}/pageSize/{pageSize:int}", async ([FromServices] IOrderService orderService, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
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
                var result = await orderService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllOrdersPaged")
               .WithSummary("Tüm siparişleri sayfalandırarak getirir");

            group.MapGet("/", async ([FromServices] IOrderService orderService, CancellationToken cancellationToken) =>
            {
                var result = await orderService.GetAsync(cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllOrders")
               .WithSummary("Tüm siparişleri getirir");

            group.MapGet("/{id:guid}", async (Guid id, [FromServices] IOrderService orderService, CancellationToken cancellationToken) =>
            {
                var result = await orderService.GetByIdAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
                .WithName("GetOrderById")
                .WithSummary("Belirli bir sipariş detayını getirir");

            group.MapPost("/", async ([FromBody] CreateOrderDto dto, [FromServices] IOrderService orderService, CancellationToken cancellationToken) =>
            {
                var result = await orderService.CreateAsync(dto, cancellationToken);
                return result.ToGenericResult();
            })
            .WithName("CreateOrder")
            .WithSummary("Yeni bir sipariş oluşturur")
            .AddEndpointFilter<ValidationFilter<CreateOrderDto>>();

            group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateOrderDto dto, [FromServices] IOrderService orderService, CancellationToken cancellationToken) =>
            {
                var result = await orderService.UpdateAsync(id, dto, cancellationToken);
                return result.ToResult();
            })
             .WithName("UpdateOrder")
             .WithSummary("Bir siparişi günceller")
             .AddEndpointFilter<ValidationFilter<UpdateOrderDto>>();

            group.MapDelete("/{id:guid}", async (Guid id, [FromServices] IOrderService orderService, CancellationToken cancellationToken) =>
            {
                var result = await orderService.DeleteAsync(id, cancellationToken);
                return result.ToResult();
            })
             .WithName("DeleteOrder")
             .WithSummary("Bir siparişi siler");

            group.MapPut("/cancel/{id:guid}", async (Guid id, [FromServices] IOrderService orderService, CancellationToken cancellationToken) =>
            {
                var result = await orderService.CancelAsync(id, cancellationToken);
                return result.ToResult();
            })
           .WithName("CancelOrder")
           .WithSummary("Bir siparişi iptal eder");
        }
    }
}