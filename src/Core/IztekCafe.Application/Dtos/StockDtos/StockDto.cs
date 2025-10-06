namespace IztekCafe.Application.Dtos.StockDtos
{
    public record StockDto(int Id, int ProductId, string ProductName, int Quantity, string Unit);
}