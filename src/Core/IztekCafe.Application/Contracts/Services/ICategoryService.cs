using IztekCafe.Application.Dtos.CategoryDtos;
using IztekCafe.Application.Dtos.Common;

namespace IztekCafe.Application.Contracts.Services
{
    public interface ICategoryService
    {
        Task<ServiceResult<IEnumerable<CategoryDto?>>> GetAsync(CancellationToken cancellationToken);

        Task<ServiceResult<PagedResult<CategoryDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<ServiceResult<CategoryDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ServiceResult> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken);

        Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken);

        Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}