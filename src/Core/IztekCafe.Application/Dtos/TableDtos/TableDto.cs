using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.TableDtos
{
    public record TableDto(int Id, string Name, TableStatus Status, string StatusName);
}