using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.TableDtos;

namespace IztekCafe.Application.Contracts.Services
{
    public interface ITableService
    {
        Task<ServiceResult<IEnumerable<TableDto?>>> GetAsync(CancellationToken cancellationToken);

        Task<ServiceResult<PagedResult<TableDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ServiceResult<TableDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateAsync(int id, UpdateTableDto dto, CancellationToken cancellationToken);

        Task<ServiceResult<TableDto>> CreateAsync(CreateTableDto dto, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult<TableOrderDto?>> GetActiveByIdWithOrderAsync(int id, CancellationToken cancellationToken);
    }
}