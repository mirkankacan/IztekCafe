using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment, Guid>
    {
    }
}