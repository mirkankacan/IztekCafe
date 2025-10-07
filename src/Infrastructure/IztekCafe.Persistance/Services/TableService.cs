using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.TableDtos;
using IztekCafe.Domain.Entities;
using Mapster;
using System.Net;

namespace IztekCafe.Persistance.Services
{
    public class TableService(IUnitOfWork unitOfWork) : ITableService
    {
        public async Task<ServiceResult<TableDto>> CreateAsync(CreateTableDto dto, CancellationToken cancellationToken)
        {
            var hasAny = await unitOfWork.Tables.AnyAsync(x => x.Name == dto.Name, cancellationToken);
            if (hasAny)
            {
                return ServiceResult<TableDto>.Error("Bu masa daha önceden oluşturulmuş", HttpStatusCode.BadRequest);
            }

            var newTable = dto.Adapt<Table>();

            await unitOfWork.Tables.AddAsync(newTable, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var createdTable = await unitOfWork.Tables.GetByIdAsync(newTable.Id, cancellationToken);
            var mappedTable = createdTable.Adapt<TableDto>();
            return ServiceResult<TableDto>.SuccessAsCreated(mappedTable, $"/api/tables/{mappedTable.Id}");
        }

        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var table = await unitOfWork.Tables.GetByIdAsync(id, cancellationToken);
            if (table is null)
            {
                return ServiceResult.Error("Masa bulunamadı", HttpStatusCode.NotFound);
            }
            unitOfWork.Tables.Remove(table);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        public async Task<ServiceResult<IEnumerable<TableDto?>>> GetAsync(CancellationToken cancellationToken)
        {
            var tables = await unitOfWork.Tables.GetAllAsync(cancellationToken);
            var mappedTables = tables?.Adapt<IEnumerable<TableDto?>>();
            return ServiceResult<IEnumerable<TableDto?>>.SuccessAsOk(mappedTables);
        }

        public async Task<ServiceResult<TableDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var table = await unitOfWork.Tables.GetByIdAsync(id, cancellationToken);
            var mappedTable = table?.Adapt<TableDetailDto?>();
            return ServiceResult<TableDetailDto?>.SuccessAsOk(mappedTable);
        }

        public async Task<ServiceResult<TableOrderDto?>> GetActiveByIdWithOrderAsync(int id, CancellationToken cancellationToken)
        {
            var table = await unitOfWork.Tables.GetActiveByIdWithOrderAsync(id, cancellationToken);
            var order = table?.Orders?.FirstOrDefault();

            if (order is null)
                return ServiceResult<TableOrderDto?>.SuccessAsOk(null);

            var mappedTableOrder = table.Adapt<TableOrderDto?>();

            return ServiceResult<TableOrderDto?>.SuccessAsOk(mappedTableOrder);
        }

        public async Task<ServiceResult<PagedResult<TableDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var tables = await unitOfWork.Tables.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            var mappedTables = tables.Data?.Adapt<IEnumerable<TableDto?>>();
            PagedResult<TableDto?> pagedResult = new(mappedTables, tables.TotalCount, pageNumber, pageSize);
            return ServiceResult<PagedResult<TableDto?>>.SuccessAsOk(pagedResult);
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateTableDto dto, CancellationToken cancellationToken)
        {
            var table = await unitOfWork.Tables.GetByIdAsync(id, cancellationToken);
            if (table is null)
            {
                return ServiceResult.Error("Masa bulunamadı", HttpStatusCode.NotFound);
            }
            var hasAnyTable = await unitOfWork.Tables.AnyAsync(x => x.Name == dto.Name && x.Id != id, cancellationToken);
            if (hasAnyTable)
            {
                return ServiceResult.Error("Masa adı kullanılıyor", HttpStatusCode.BadRequest);
            }
            var updateTable = dto.Adapt(table);
            unitOfWork.Tables.Update(updateTable);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}