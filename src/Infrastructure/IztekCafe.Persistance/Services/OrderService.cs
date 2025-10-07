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
        public async Task<ServiceResult> CancelAsync(Guid orderId, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var order = await unitOfWork.Orders.GetByIdWithItemsPaymentAndTableAsync(orderId, cancellationToken);
                if (order is null)
                {
                    return ServiceResult.Error("Sipariş bulunamadı", HttpStatusCode.NotFound);
                }
                if (order.Status == OrderStatus.Cancelled)
                {
                    return ServiceResult.Error("Sipariş zaten iptal edilmiş", HttpStatusCode.BadRequest);
                }
                var productIds = order.OrderItems.Select(x => x.ProductId).ToList();
                var stocks = await unitOfWork.Stocks.Where(x => productIds.Contains(x.ProductId)).ToListAsync(cancellationToken);

                foreach (var orderItem in order.OrderItems)
                {
                    var stock = stocks.FirstOrDefault(s => s.ProductId == orderItem.ProductId);
                    if (stock is not null)
                    {
                        stock.Quantity += orderItem.Quantity;
                    }
                }
                unitOfWork.Stocks.UpdateRange(stocks);

                await unitOfWork.Orders.UpdateStatusAsync(orderId, OrderStatus.Cancelled, cancellationToken);
                await unitOfWork.Tables.UpdateStatusAsync(order.TableId, TableStatus.Available, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return ServiceResult.SuccessAsNoContent();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ServiceResult.Error("Sipariş iptal etme başarısız", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<OrderDto>> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var hasTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId, cancellationToken);
                if (!hasTable)
                {
                    return ServiceResult<OrderDto>.Error("Sipariş için masa bulunamadı", HttpStatusCode.NotFound);
                }
                var isOccupiedTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId && x.Status == TableStatus.Occupied, cancellationToken);

                if (isOccupiedTable)
                {
                    return ServiceResult<OrderDto>.Error("Sipariş için masa dolu", HttpStatusCode.BadRequest);
                }
                if (!dto.OrderItems.Any())
                {
                    return ServiceResult<OrderDto>.Error("Sipariş için en az bir ürün eklenmeli", HttpStatusCode.BadRequest);
                }
                var newOrder = dto.Adapt<Order>();
                newOrder.OrderCode = GenerateOrderCode();

                await unitOfWork.Orders.AddAsync(newOrder, cancellationToken);

                var orderItems = new List<OrderItem>();
                foreach (var item in dto.OrderItems)
                {
                    var product = await unitOfWork.Products.GetFirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);
                    if (product is null)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ServiceResult<OrderDto>.Error("Sipariş için ürün bulunamadı", HttpStatusCode.NotFound);
                    }

                    var hasStock = await unitOfWork.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Quantity >= item.Quantity, cancellationToken);
                    if (!hasStock)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ServiceResult<OrderDto>.Error($"Sipariş için {product.Name} ürününün yetersiz stoğu var", HttpStatusCode.BadRequest);
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price
                    };

                    orderItems.Add(orderItem);
                    newOrder.TotalAmount += orderItem.TotalPrice;
                }

                await unitOfWork.OrderItems.AddRangeAsync(orderItems, cancellationToken);

                foreach (var item in dto.OrderItems)
                {
                    await unitOfWork.Stocks.DecreaseStockAsync(item.ProductId, item.Quantity, cancellationToken);
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

        public async Task<ServiceResult> UpdateAsync(Guid id, UpdateOrderDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync(cancellationToken);
                var existingOrder = await unitOfWork.Orders.GetByIdWithItemsPaymentAndTableAsync(id, cancellationToken);
                if (existingOrder is null)
                {
                    return ServiceResult.Error("Sipariş bulunamadı", HttpStatusCode.NotFound);
                }
                if (existingOrder.Status == OrderStatus.Cancelled || existingOrder.Status == OrderStatus.Paid)
                {
                    return ServiceResult.Error("İptal edilmiş veya ödenmiş sipariş güncellenemez", HttpStatusCode.BadRequest);
                }
                var hasTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId, cancellationToken);
                if (!hasTable)
                {
                    return ServiceResult.Error("Sipariş için masa bulunamadı", HttpStatusCode.NotFound);
                }

                if (existingOrder.TableId != dto.TableId)
                {
                    var isOccupiedTable = await unitOfWork.Tables.AnyAsync(x => x.Id == dto.TableId && x.Status == TableStatus.Occupied, cancellationToken);
                    if (isOccupiedTable)
                    {
                        return ServiceResult.Error("Sipariş için masa dolu", HttpStatusCode.BadRequest);
                    }
                }

                if (!dto.OrderItems.Any())
                {
                    return ServiceResult.Error("Sipariş için en az bir ürün eklenmeli", HttpStatusCode.BadRequest);
                }
                // Eski sipariş kalemlerini kaldır ve stokları geri ekle
                var oldProductIds = existingOrder.OrderItems.Select(x => x.ProductId).ToList();
                var oldStocks = await unitOfWork.Stocks.Where(x => oldProductIds.Contains(x.ProductId)).ToListAsync(cancellationToken);

                foreach (var oldItem in existingOrder.OrderItems)
                {
                    var stock = oldStocks.FirstOrDefault(s => s.ProductId == oldItem.ProductId);
                    if (stock is not null)
                    {
                        stock.Quantity += oldItem.Quantity;
                    }
                }

                unitOfWork.Stocks.UpdateRange(oldStocks);
                unitOfWork.OrderItems.RemoveRange(existingOrder.OrderItems);

                // Yeni sipariş kalemlerini ekle ve stokları güncelle
                var newProductIds = dto.OrderItems.Select(x => x.ProductId).ToList();
                var products = await unitOfWork.Products.Where(x => newProductIds.Contains(x.Id)).ToListAsync(cancellationToken);
                var newStocks = await unitOfWork.Stocks.Where(x => newProductIds.Contains(x.ProductId)).ToListAsync(cancellationToken);

                existingOrder.TotalAmount = 0;
                var newOrderItems = new List<OrderItem>();

                foreach (var item in dto.OrderItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product is null)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ServiceResult.Error("Sipariş için ürün bulunamadı", HttpStatusCode.NotFound);
                    }

                    var stock = newStocks.FirstOrDefault(s => s.ProductId == item.ProductId);
                    if (stock is null || stock.Quantity < item.Quantity)
                    {
                        await unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return ServiceResult.Error($"Sipariş için {product.Name} ürününün yetersiz stoğu var", HttpStatusCode.BadRequest);
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = existingOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price
                    };
                    newOrderItems.Add(orderItem);

                    stock.Quantity -= item.Quantity;
                    existingOrder.TotalAmount += orderItem.TotalPrice;
                }

                await unitOfWork.OrderItems.AddRangeAsync(newOrderItems, cancellationToken);
                unitOfWork.Stocks.UpdateRange(newStocks);

                existingOrder.TableId = dto.TableId;
                // Masa değişimi
                if (existingOrder.TableId != dto.TableId)
                {
                    await unitOfWork.Tables.UpdateStatusAsync(existingOrder.TableId, TableStatus.Available, cancellationToken);
                }
                await unitOfWork.Tables.UpdateStatusAsync(dto.TableId, TableStatus.Occupied, cancellationToken);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(cancellationToken);

                return ServiceResult.SuccessAsNoContent();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ServiceResult.Error("Sipariş güncelleme başarısız", HttpStatusCode.InternalServerError);
            }
        }

        private string GenerateOrderCode()
        {
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            string code = $"IZTEKCAFE-{guid.Substring(0, 6)}";
            return code;
        }
    }
}