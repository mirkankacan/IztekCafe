using IztekCafe.Application.Dtos.PaymentDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Extensions;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class PaymentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Payment, PaymentDto>()
                .Map(dest => dest.OrderCode, src => src.Order.OrderCode)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());

            config.NewConfig<Payment, PaymentDetailDto>()
                .Map(dest => dest.OrderCode, src => src.Order.OrderCode)
                .Map(dest => dest.OrderItems, src => src.Order.OrderItems)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());
        }
    }
}