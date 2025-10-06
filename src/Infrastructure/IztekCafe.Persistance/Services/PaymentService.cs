using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.PaymentDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using Mapster;
using System.Net;

namespace IztekCafe.Persistance.Services
{
    public class PaymentService(IUnitOfWork unitOfWork) : IPaymentService
    {
        public async Task<ServiceResult<PaymentDto>> CreateAsync(CreatePaymentDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var hasOrder = await unitOfWork.Orders.AnyAsync(x => x.Id == dto.OrderId, cancellationToken);
                if (!hasOrder)
                {
                    return ServiceResult<PaymentDto>.Error("Ödeme için sipariş bulunamadı", HttpStatusCode.BadRequest);
                }
                var newPayment = dto.Adapt<Payment>();
                await unitOfWork.Payments.AddAsync(newPayment, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var bankResponse = await BankPaymentRequest();
                if (!bankResponse)
                {
                    await unitOfWork.Payments.UpdateStatusAsync(newPayment.Id, PaymentStatus.Failed, cancellationToken);
                    return ServiceResult<PaymentDto>.Error("Ödeme için bankadan dönen cevap başarısız", HttpStatusCode.BadRequest);
                }
                await unitOfWork.Payments.UpdateStatusAsync(newPayment.Id, PaymentStatus.Completed, cancellationToken);
                await unitOfWork.Orders.UpdateStatusAsync(dto.OrderId, OrderStatus.Completed, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(cancellationToken);
                var createdPayment = await unitOfWork.Payments.GetByIdWithOrder(newPayment.Id, cancellationToken);
                var mappedPayment = createdPayment.Adapt<PaymentDto>();
                return ServiceResult<PaymentDto>.SuccessAsCreated(mappedPayment, $"/api/payments/{mappedPayment.Id}");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ServiceResult<PaymentDto>.Error("Ödeme başarısız", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<IEnumerable<PaymentDto?>>> GetAsync(CancellationToken cancellationToken)
        {
            var payments = await unitOfWork.Payments.GetWithOrder(cancellationToken);
            var mappedPayments = payments.Adapt<IEnumerable<PaymentDto?>>();
            return ServiceResult<IEnumerable<PaymentDto?>>.SuccessAsOk(mappedPayments);
        }

        public async Task<ServiceResult<PaymentDetailDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var payment = await unitOfWork.Payments.GetByIdWithOrder(id, cancellationToken);
            var mappedPayment = payment.Adapt<PaymentDetailDto?>();
            return ServiceResult<PaymentDetailDto?>.SuccessAsOk(mappedPayment);
        }

        public async Task<ServiceResult<PagedResult<PaymentDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var payments = await unitOfWork.Categories.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            var mappedPayments = payments.Data?.Adapt<IEnumerable<PaymentDto?>>();
            PagedResult<PaymentDto?> pagedResult = new(mappedPayments, payments.TotalCount, pageNumber, pageSize);
            return ServiceResult<PagedResult<PaymentDto?>>.SuccessAsOk(pagedResult);
        }

        private async Task<bool> BankPaymentRequest()
        {
            await Task.Delay(3000);
            return true;
        }
    }
}