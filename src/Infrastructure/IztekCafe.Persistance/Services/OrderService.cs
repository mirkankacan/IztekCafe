using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.OrderDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IztekCafe.Persistance.Services
{
    public class OrderService(IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<ServiceResult> CancelOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            await unitOfWork.Orders.UpdateStatusAsync(orderId, OrderStatus.Cancelled, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        public async Task<ServiceResult<OrderDto>> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var hasTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId, cancellationToken);
                if (!hasTable)
                {
                    return ServiceResult<OrderDto>.Error("Sipariş için masa bulunamadı", HttpStatusCode.BadRequest);
                }
                var isOccupiedTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId && (x.Status == TableStatus.Reserved || x.Status == TableStatus.Occupied), cancellationToken);

                if (isOccupiedTable)
                {
                    return ServiceResult<OrderDto>.Error("Sipariş için masa dolu veya rezerve edilmiş", HttpStatusCode.BadRequest);
                }

                var newOrder = dto.Adapt<Order>();
                newOrder.OrderCode = GenerateOrderCode();

                await unitOfWork.Orders.AddAsync(newOrder, cancellationToken);

                foreach (var item in dto.OrderItems)
                {
                    var product = await unitOfWork.Products.GetFirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);
                    if (product is null)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ServiceResult<OrderDto>.Error("Sipariş için ürün bulunamadı", HttpStatusCode.BadRequest);
                    }
                    var hasStock = await unitOfWork.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Quantity >= item.Quantity, cancellationToken);

                    if (!hasStock)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ServiceResult<OrderDto>.Error("Sipariş için ürünün yetersiz stoğu var", HttpStatusCode.BadRequest);
                    }
                    var orderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price
                    };

                    await unitOfWork.OrderItems.AddAsync(orderItem, cancellationToken);
                    await unitOfWork.Stocks.DecreaseStockAsync(item.ProductId, item.Quantity, cancellationToken);
                    newOrder.TotalAmount += orderItem.TotalPrice;
                }
                await unitOfWork.Tables.UpdateStatusAsync(dto.TableId, TableStatus.Occupied, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);

                return ServiceResult<OrderDto>.SuccessAsCreated(newOrder.Adapt<OrderDto>(), $"/api/orders/{newOrder.Id}");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ServiceResult<OrderDto>.Error("Sipariş oluşturma başarısız", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);

                var order = await unitOfWork.Orders.GetByIdAsync(id, cancellationToken);
                if (order is null)
                {
                    return ServiceResult.Error("Sipariş bulunamadı", HttpStatusCode.NotFound);
                }

                var orderItems = await unitOfWork.OrderItems.Where(x => x.OrderId == order.Id).ToListAsync(cancellationToken);

                if (orderItems.Any())
                {
                    foreach (var item in orderItems)
                    {
                        await unitOfWork.Stocks.IncreaseStockAsync(item.ProductId, item.Quantity, cancellationToken);
                    }
                    unitOfWork.OrderItems.RemoveRange(orderItems);
                }

                // Masayı boşalt
                await unitOfWork.Tables.UpdateStatusAsync(order.TableId, TableStatus.Available, cancellationToken);

                unitOfWork.Orders.Remove(order);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);

                return ServiceResult.SuccessAsNoContent();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ServiceResult.Error("Sipariş silme başarısız", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<IEnumerable<OrderDto?>>> GetAsync(CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.Orders.GetWithItemsPaymentAndTableAsync(cancellationToken);
            var mappedOrders = orders?.Adapt<IEnumerable<OrderDto?>>();
            return ServiceResult<IEnumerable<OrderDto?>>.SuccessAsOk(mappedOrders);
        }

        public async Task<ServiceResult<OrderDetailDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Orders.GetByIdWithItemsPaymentAndTableAsync(id, cancellationToken);
            var mappedOrder = order?.Adapt<OrderDetailDto?>();
            return ServiceResult<OrderDetailDto?>.SuccessAsOk(mappedOrder);
        }

        public async Task<ServiceResult<PagedResult<OrderDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.Orders.GetPagedWithItemsPaymentAndTableAsync(pageNumber, pageSize, cancellationToken);
            var mappedOrders = orders.Data?.Adapt<IEnumerable<OrderDto?>>();
            PagedResult<OrderDto?> pagedResult = new(mappedOrders, orders.TotalCount, pageNumber, pageSize);
            return ServiceResult<PagedResult<OrderDto?>>.SuccessAsOk(pagedResult);
        }

        public async Task<ServiceResult<OrderDto>> UpdateAsync(Guid id, UpdateOrderDto dto, CancellationToken cancellationToken)
        {
            throw new Exception("Not implemented yet");
            //try
            //{
            //    await unitOfWork.BeginTransactionAsync(cancellationToken);

            //    var existingOrder = await unitOfWork.Orders.GetWithItemsAsync(id, cancellationToken);
            //    if (existingOrder == null)
            //    {
            //        return ServiceResult<OrderDto>.Error("Sipariş bulunamadı", HttpStatusCode.NotFound);
            //    }

            //    var hasTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId, cancellationToken);
            //    if (!hasTable)
            //    {
            //        return ServiceResult<OrderDto>.Error("Masa bulunamadı", HttpStatusCode.BadRequest);
            //    }
            //    var isOccupiedTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId && (x.Status == TableStatus.Reserved || x.Status == TableStatus.Occupied), cancellationToken);

            //    if (isOccupiedTable)
            //    {
            //        return ServiceResult<OrderDto>.Error("Sipariş için masa dolu veya rezerve edilmiş", HttpStatusCode.BadRequest);
            //    }
            //    existingOrder.TableId = dto.TableId;

            //    var oldItems = existingOrder.OrderItems.ToDictionary(x => x.ProductId, x => x);

            //    foreach (var itemDto in dto.OrderItems)
            //    {
            //        var productExists = await unitOfWork.Products.AnyAsync(x => x.Id == itemDto.ProductId, cancellationToken);
            //        if (!productExists)
            //        {
            //            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            //            return ServiceResult<OrderDto>.Error($"Ürün bulunamadı: {itemDto.ProductId}", HttpStatusCode.BadRequest);
            //        }

            //        if (oldItems.TryGetValue(itemDto.ProductId, out var existingItem))
            //        {
            //            // Miktar farkını hesapla
            //            int fark = itemDto.Quantity - existingItem.Quantity;

            //            if (fark > 0)
            //                await unitOfWork.Stocks.DecreaseStockAsync(itemDto.ProductId, fark, cancellationToken);
            //            else if (fark < 0)
            //                await unitOfWork.Stocks.IncreaseStockAsync(itemDto.ProductId, Math.Abs(fark), cancellationToken);

            //            // Mevcut item'ı güncelle
            //            existingItem.Quantity = itemDto.Quantity;
            //            existingItem.Price = itemDto.Price;
            //            await unitOfWork.OrderItems.UpdateAsync(existingItem, cancellationToken);

            //            oldItems.Remove(itemDto.ProductId); // işlenenleri çıkar
            //        }
            //        else
            //        {
            //            // Yeni ürün eklendi
            //            await unitOfWork.Stocks.DecreaseStockAsync(itemDto.ProductId, itemDto.Quantity, cancellationToken);

            //            var newItem = itemDto.Adapt<OrderItem>();
            //            newItem.OrderId = existingOrder.Id;
            //            await unitOfWork.OrderItems.AddAsync(newItem, cancellationToken);
            //        }
            //    }

            //    // Artık DTO'da olmayan ürünleri kaldır → stoğu geri ekle
            //    foreach (var removedItem in oldItems.Values)
            //    {
            //        await unitOfWork.Stocks.IncreaseStockAsync(removedItem.ProductId, removedItem.Quantity, cancellationToken);
            //        await unitOfWork.OrderItems.DeleteAsync(removedItem, cancellationToken);
            //    }

            //    await unitOfWork.SaveChangesAsync(cancellationToken);
            //    await unitOfWork.CommitTransactionAsync(cancellationToken);

            //    return ServiceResult<OrderDto>.SuccessAsOk(existingOrder.Adapt<OrderDto>());
            //}
            //catch (Exception)
            //{
            //    await unitOfWork.RollbackTransactionAsync(cancellationToken);
            //    return ServiceResult<OrderDto>.Error("Sipariş güncelleme başarısız", HttpStatusCode.InternalServerError);
            //}
        }

        private string GenerateOrderCode()
        {
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            string code = $"IZTEKCAFE-{guid.Substring(0, 6)}";
            return code;
        }
    }
}