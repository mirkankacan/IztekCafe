using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;

namespace IztekCafe.Persistance.Repositories
{
    public class PaymentRepository(AppDbContext context) : GenericRepository<Payment, Guid>(context), IPaymentRepository
    {
    }
}