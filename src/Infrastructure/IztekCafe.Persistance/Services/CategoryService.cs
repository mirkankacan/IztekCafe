using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Application.Dtos.CategoryDtos;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using Mapster;
using System.Net;

namespace IztekCafe.Persistance.Services
{
    public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
    {
        public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken)
        {
            var hasAny = await unitOfWork.Categories.AnyAsync(x => x.Name == dto.Name, cancellationToken);
            if (hasAny)
            {
                return ServiceResult<CategoryDto>.Error("Kategori adı kullanılıyor", HttpStatusCode.BadRequest);
            }

            var newCategory = dto.Adapt<Category>();
            await unitOfWork.Categories.AddAsync(newCategory, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var createdCategory = await unitOfWork.Categories.GetByIdWithProductsAsync(newCategory.Id, cancellationToken);
            var mappedCategory = createdCategory.Adapt<CategoryDto>();
            return ServiceResult<CategoryDto>.SuccessAsCreated(mappedCategory, $"/api/categories/{mappedCategory.Id}");
        }

        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
            if (category is null)
            {
                return ServiceResult.Error("Kategori bulunamadı", HttpStatusCode.NotFound);
            }
            unitOfWork.Categories.Remove(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        public async Task<ServiceResult<IEnumerable<CategoryDto?>>> GetAsync(CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.GetAllAsync(cancellationToken);
            var mappedCategories = categories?.Adapt<IEnumerable<CategoryDto?>>();
            return ServiceResult<IEnumerable<CategoryDto?>>.SuccessAsOk(mappedCategories);
        }

        public async Task<ServiceResult<CategoryDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdWithProductsAsync(id, cancellationToken);
            var mappedCategory = category?.Adapt<CategoryDetailDto?>();
            return ServiceResult<CategoryDetailDto?>.SuccessAsOk(mappedCategory);
        }

        public async Task<ServiceResult<PagedResult<CategoryDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.GetAllPagedAsync(pageNumber, pageSize, cancellationToken);
            var mappedCategories = categories.Data?.Adapt<IEnumerable<CategoryDto?>>();
            PagedResult<CategoryDto?> pagedResult = new(mappedCategories, categories.TotalCount, pageNumber, pageSize);
            return ServiceResult<PagedResult<CategoryDto?>>.SuccessAsOk(pagedResult);
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
            if (category is null)
            {
                return ServiceResult.Error("Kategori bulunamadı", HttpStatusCode.NotFound);
            }
            var hasAny = await unitOfWork.Categories.AnyAsync(x => x.Name == dto.Name && x.Id != id, cancellationToken);
            if (hasAny)
            {
                return ServiceResult.Error("Kategori adı kullanılıyor", HttpStatusCode.BadRequest);
            }
            var updateCategory = dto.Adapt(category);
            unitOfWork.Categories.Update(updateCategory);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}