using Carter;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Dtos.TableDtos;
using IztekCafe.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace IztekCafe.WebApi.Endpoints
{
    public class TableEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/tables")
                .WithTags("Tables")
                .WithOpenApi();

            group.MapGet("/pageNumber/{pageNumber:int}/pageSize/{pageSize:int}", async ([FromServices] ITableService tableService, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
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
                var result = await tableService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllTablesPaged")
               .WithSummary("Tüm masaları sayfalandırarak getirir");

            group.MapGet("/", async ([FromServices] ITableService tableService, CancellationToken cancellationToken) =>
            {
                var result = await tableService.GetAsync(cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllTables")
               .WithSummary("Tüm masaları getirir");

            group.MapGet("/{id:int}", async (int id, [FromServices] ITableService tableService, CancellationToken cancellationToken) =>
            {
                var result = await tableService.GetByIdAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
                .WithName("GetTableById")
                .WithSummary("Belirli bir masayı getirir");
            group.MapGet("/{id:int}/active-with-order", async (int id, [FromServices] ITableService tableService, CancellationToken cancellationToken) =>
            {
                var result = await tableService.GetActiveByIdWithOrderAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
             .WithName("GetActiveTableByIdWithOrder")
             .WithSummary("Belirli bir masayı aktif siparişiyle getirir");

            group.MapPost("/", async ([FromBody] CreateTableDto dto, [FromServices] ITableService tableService, CancellationToken cancellationToken) =>
            {
                var result = await tableService.CreateAsync(dto, cancellationToken);
                return result.ToGenericResult();
            })
            .WithName("CreateTable")
            .WithSummary("Yeni bir masa oluşturur");

            group.MapPut("/{id:int}", async (int id, [FromBody] UpdateTableDto dto, [FromServices] ITableService tableService, CancellationToken cancellationToken) =>
            {
                var result = await tableService.UpdateAsync(id, dto, cancellationToken);
                return result.ToResult();
            })
             .WithName("UpdateTable")
             .WithSummary("Bir masayı günceller");

            group.MapDelete("/{id:int}", async (int id, [FromServices] ITableService tableService, CancellationToken cancellationToken) =>
            {
                var result = await tableService.DeleteAsync(id, cancellationToken);
                return result.ToResult();
            })
           .WithName("DeleteTable")
           .WithSummary("Bir masayı siler");
        }
    }
}