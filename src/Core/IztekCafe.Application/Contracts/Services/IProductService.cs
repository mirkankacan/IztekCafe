using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.ProductDtos;

namespace IztekCafe.Application.Contracts.Services
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<ProductDto?>>> GetAsync(CancellationToken cancellationToken);

        Task<ServiceResult<IEnumerable<ProductDto?>>> GetActivesAsync(CancellationToken cancellationToken);

        Task<ServiceResult<PagedResult<ProductDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ServiceResult<ProductDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken);

        Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}