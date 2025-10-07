using IztekCafe.Application.Dtos.OrderDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Domain.Extensions;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class OrderMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Order, OrderDto>()
                .Map(dest => dest.TableName, src => src.Table.Name)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString())
                .Map(dest => dest.PaymentStatus, src => src.Payment != null ? src.Payment.Status : PaymentStatus.Pending)
                .Map(dest => dest.PaymentStatusName, src => src.Payment != null ? src.Payment.Status.ToDisplayString() : PaymentStatus.Pending.ToDisplayString());

            config.NewConfig<Order, OrderDetailDto>()
                .Map(dest => dest.OrderItems, src => src.OrderItems.ToList())
                .Map(dest => dest.TableName, src => src.Table.Name)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString())
                .Map(dest => dest.PaymentStatus, src => src.Payment != null ? src.Payment.Status : PaymentStatus.Pending)
                .Map(dest => dest.PaymentStatusName, src => src.Payment != null ? src.Payment.Status.ToDisplayString() : PaymentStatus.Pending.ToDisplayString());
        }
    }
}