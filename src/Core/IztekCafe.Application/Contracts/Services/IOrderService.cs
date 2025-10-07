using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.OrderDtos;

namespace IztekCafe.Application.Contracts.Services
{
    public interface IOrderService
    {
        Task<ServiceResult<IEnumerable<OrderDto?>>> GetAsync(CancellationToken cancellationToken);

        Task<ServiceResult<PagedResult<OrderDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ServiceResult<OrderDetailDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateAsync(Guid id, UpdateOrderDto dto, CancellationToken cancellationToken);

        Task<ServiceResult<OrderDto>> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<ServiceResult> CancelAsync(Guid orderId, CancellationToken cancellationToken);
    }
}