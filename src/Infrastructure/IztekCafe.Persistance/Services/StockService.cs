using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.StockDtos;
using IztekCafe.Domain.Entities;
using Mapster;
using System.Net;

namespace IztekCafe.Persistance.Services
{
    public class StockService(IUnitOfWork unitOfWork) : IStockService
    {
        public async Task<ServiceResult<StockDto>> CreateAsync(CreateStockDto dto, CancellationToken cancellationToken)
        {
            var hasAny = await unitOfWork.Stocks.AnyAsync(x => x.ProductId == dto.ProductId, cancellationToken);
            if (hasAny)
            {
                return ServiceResult<StockDto>.Error("Ürün için stok kaydı oluşturulmuş", HttpStatusCode.BadRequest);
            }
            var hasProduct = await unitOfWork.Products.AnyAsync(x => x.Id == dto.ProductId, cancellationToken);
            if (!hasProduct)
            {
                return ServiceResult<StockDto>.Error("Stok oluşturmak için ilgili ürün bulunamadı", HttpStatusCode.BadRequest);
            }
            var newStock = dto.Adapt<Stock>();

            await unitOfWork.Stocks.AddAsync(newStock, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var createdStock = await unitOfWork.Stocks.GetByIdWithProductAsync(newStock.Id, cancellationToken);
            var mappedStock = createdStock.Adapt<StockDto>();
            return ServiceResult<StockDto>.SuccessAsCreated(mappedStock, $"/api/stocks/{mappedStock.Id}");
        }

        public async Task<ServiceResult<bool>> DecreaseAsync(int productId, int quantity, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.Stocks.DecreaseStockAsync(productId, quantity, cancellationToken);
            if (!result)
            {
                return ServiceResult<bool>.Error("Yetersiz stok ya da stok bulunamadı", HttpStatusCode.BadRequest);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.SuccessAsOk(true);
        }

        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var stock = await unitOfWork.Stocks.GetByIdAsync(id, cancellationToken);
            if (stock is null)
            {
                return ServiceResult.Error("Stok bulunamadı", HttpStatusCode.NotFound);
            }
            unitOfWork.Stocks.Remove(stock);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        public async Task<ServiceResult<IEnumerable<StockDto?>>> GetAsync(CancellationToken cancellationToken)
        {
            var stocks = await unitOfWork.Stocks.GetWithProductAsync(cancellationToken);
            var mappedstocks = stocks?.Adapt<IEnumerable<StockDto?>>();
            return ServiceResult<IEnumerable<StockDto?>>.SuccessAsOk(mappedstocks);
        }

        public async Task<ServiceResult<StockDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var stock = await unitOfWork.Stocks.GetByIdWithProductAsync(id, cancellationToken);
            var mappedstock = stock?.Adapt<StockDetailDto?>();
            return ServiceResult<StockDetailDto?>.SuccessAsOk(mappedstock);
        }

        public async Task<ServiceResult<PagedResult<StockDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var stocks = await unitOfWork.Stocks.GetPagedWithProductAsync(pageNumber, pageSize, cancellationToken);
            var mappedstocks = stocks.Data?.Adapt<IEnumerable<StockDto?>>();
            PagedResult<StockDto?> pagedResult = new(mappedstocks, stocks.TotalCount, pageNumber, pageSize);
            return ServiceResult<PagedResult<StockDto?>>.SuccessAsOk(pagedResult);
        }

        public async Task<ServiceResult<bool>> IncreaseAsync(int productId, int quantity, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.Stocks.IncreaseStockAsync(productId, quantity, cancellationToken);
            if (!result)
            {
                return ServiceResult<bool>.Error("Stok bulunamadı", HttpStatusCode.BadRequest);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.SuccessAsOk(true);
        }

        public async Task<ServiceResult<StockDto>> UpdateAsync(int id, UpdateStockDto dto, CancellationToken cancellationToken)
        {
            var stock = await unitOfWork.Stocks.GetByIdAsync(id, cancellationToken);
            if (stock is null)
            {
                return ServiceResult<StockDto>.Error("Ürün bulunamadı", HttpStatusCode.NotFound);
            }
            var hasAny = await unitOfWork.Stocks.AnyAsync(x => x.ProductId == dto.ProductId && x.Id != id, cancellationToken);
            if (hasAny)
            {
                return ServiceResult<StockDto>.Error("Ürün için stok kaydı oluşturulmuş", HttpStatusCode.BadRequest);
            }
            var updateStock = dto.Adapt(stock);
            unitOfWork.Stocks.Update(updateStock);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedStock = await unitOfWork.Stocks.GetByIdWithProductAsync(id, cancellationToken);
            var mappedstock = updatedStock.Adapt<StockDto>();
            return ServiceResult<StockDto>.SuccessAsOk(mappedstock);
        }
    }
}