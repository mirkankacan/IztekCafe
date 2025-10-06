using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment, Guid>
    {
        Task<bool> UpdateStatusAsync(Guid id, PaymentStatus status, CancellationToken cancellationToken);

        Task<IEnumerable<Payment?>> GetWithOrder(CancellationToken cancellationToken);

        Task<PagedResult<Payment?>> GetPagedWithOrderAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<Payment?> GetByIdWithOrder(Guid id, CancellationToken cancellationToken);
    }
}