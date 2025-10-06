using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.StockDtos;

namespace IztekCafe.Application.Contracts.Services
{
    public interface IStockService
    {
        Task<ServiceResult<IEnumerable<StockDto?>>> GetAsync(CancellationToken cancellationToken);

        Task<ServiceResult<PagedResult<StockDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ServiceResult<StockDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult<StockDto>> UpdateAsync(int id, UpdateStockDto dto, CancellationToken cancellationToken);

        Task<ServiceResult<StockDto>> CreateAsync(CreateStockDto dto, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult<bool>> IncreaseAsync(int productId, int quantity, CancellationToken cancellationToken);

        Task<ServiceResult<bool>> DecreaseAsync(int productId, int quantity, CancellationToken cancellationToken);
    }
}