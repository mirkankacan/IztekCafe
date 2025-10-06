using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.PaymentDtos;

namespace IztekCafe.Application.Contracts.Services
{
    public interface IPaymentService
    {
        Task<ServiceResult<IEnumerable<PaymentDto?>>> GetAsync(CancellationToken cancellationToken);

        Task<ServiceResult<PagedResult<PaymentDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ServiceResult<PaymentDetailDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ServiceResult<PaymentDto>> CreateAsync(CreatePaymentDto dto, CancellationToken cancellationToken);
    }
}