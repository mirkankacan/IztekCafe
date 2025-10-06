using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.TableDtos
{
    public record TableDetailDto(int Id, string Name, TableStatus Status, string StatusName, DateTime CreatedAt, DateTime? UpdatedAt);
}