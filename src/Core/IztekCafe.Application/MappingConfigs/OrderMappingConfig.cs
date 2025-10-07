using IztekCafe.Application.Dtos.OrderDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Extensions;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class OrderMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateOrderDto, Order>()
               .Ignore(dest => dest.Payment)
               .Ignore(dest => dest.OrderItems);
            config.NewConfig<Order, OrderDto>()
                 .Map(dest => dest.TableName, src => src.Table.Name)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString())
                .Map(dest => dest.PaymentStatusName, src => src.Payment != null ? src.Payment.Status.ToDisplayString() : null);

            config.NewConfig<Order, OrderDetailDto>()
                .Map(dest => dest.OrderItems, src => src.OrderItems.ToList())
                .Map(dest => dest.TableName, src => src.Table.Name)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString())
                .Map(dest => dest.PaymentStatusName, src => src.Payment != null ? src.Payment.Status.ToDisplayString() : null);
        }
    }
}
