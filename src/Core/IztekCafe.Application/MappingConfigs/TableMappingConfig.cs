using IztekCafe.Application.Dtos.OrderItemDtos;
using IztekCafe.Application.Dtos.TableDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Extensions;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class TableMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Table, TableDto>()
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());

            config.NewConfig<Table, TableDetailDto>()
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());

            config.NewConfig<Table, TableOrderDto?>()
                .MapWith(src => MapToTableOrderDto(src));
        }
        private static TableOrderDto? MapToTableOrderDto(Table table)
        {
            var order = table?.Orders?.FirstOrDefault();
            if (order == null)
                return null;

            var orderItemsDto = order.OrderItems?.Adapt<List<OrderItemDto>>();

            return new TableOrderDto(
                table.Id,
                table.Status,
                table.Status.ToDisplayString(),
                order.Id,
                order.OrderCode,
                order.Status,
                order.Status.ToDisplayString(),
                order.TotalAmount,
                orderItemsDto,
                order.Payment?.Id,
                order.Payment?.Status,
                order.Payment?.Status.ToDisplayString(),
                order.CreatedAt
            );
        }
    }
}