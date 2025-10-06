namespace IztekCafe.Application.Dtos.StockDtos
{
    public record StockDetailDto(int Id, int ProductId, string ProductName, int Quantity, string Unit, DateTime CreatedAt, DateTime? UpdatedAt);
}