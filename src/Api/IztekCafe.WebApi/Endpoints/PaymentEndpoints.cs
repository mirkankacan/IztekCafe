using Carter;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Dtos.PaymentDtos;
using IztekCafe.WebApi.Extensions;
using IztekCafe.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IztekCafe.WebApi.Endpoints
{
    public class PaymentEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/payments")
                .WithTags("Payments")
                .WithOpenApi();

            group.MapGet("/pageNumber/{pageNumber:int}/pageSize/{pageSize:int}", async ([FromServices] IPaymentService paymentService, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
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
                var result = await paymentService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllPaymentsPaged")
               .WithSummary("Tüm ürünleri sayfalandırarak getirir");

            group.MapGet("/", async ([FromServices] IPaymentService paymentService, CancellationToken cancellationToken) =>
            {
                var result = await paymentService.GetAsync(cancellationToken);
                return result.ToGenericResult();
            })
               .WithName("GetAllPayments")
               .WithSummary("Tüm ödemeleri getirir");

            group.MapGet("/{id:guid}", async (Guid id, [FromServices] IPaymentService paymentService, CancellationToken cancellationToken) =>
            {
                var result = await paymentService.GetByIdAsync(id, cancellationToken);
                return result.ToGenericResult();
            })
                .WithName("GetPaymentById")
                .WithSummary("Belirli bir ödemeyi getirir");

            group.MapPost("/", async ([FromBody] CreatePaymentDto dto, [FromServices] IPaymentService paymentService, CancellationToken cancellationToken) =>
            {
                var result = await paymentService.CreateAsync(dto, cancellationToken);
                return result.ToGenericResult();
            })
            .WithName("CreatePayment")
            .WithSummary("Ödeme oluşturur")
            .AddEndpointFilter<ValidationFilter<CreatePaymentDto>>();
        }
    }
}