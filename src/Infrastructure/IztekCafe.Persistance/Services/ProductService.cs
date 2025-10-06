using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Application.Dtos.ProductDtos;
using IztekCafe.Domain.Entities;
using Mapster;
using System.Net;

namespace IztekCafe.Persistance.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ServiceResult<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken)
        {
            var hasAnyProduct = await unitOfWork.Products.AnyAsync(x => x.Name == dto.Name, cancellationToken);
            if (hasAnyProduct)
            {
                return ServiceResult<ProductDto>.Error("Ürün adı kullanılıyor", HttpStatusCode.BadRequest);
            }
            var hasAnyCategory = await unitOfWork.Categories.AnyAsync(x => x.Id == dto.CategoryId, cancellationToken);
            if (!hasAnyCategory)
            {
                return ServiceResult<ProductDto>.Error("Kategori bulunamadı", HttpStatusCode.BadRequest);
            }
            var newProduct = dto.Adapt<Product>();

            await unitOfWork.Products.AddAsync(newProduct, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var createdProduct = await unitOfWork.Products.GetByIdWithCategoryAndStockAsync(newProduct.Id, cancellationToken);
            var mappedProduct = createdProduct.Adapt<ProductDto>();
            return ServiceResult<ProductDto>.SuccessAsCreated(mappedProduct, $"/api/products/{mappedProduct.Id}");
        }

        public async Task<ServiceResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return ServiceResult.Error("Ürün bulunamadı", HttpStatusCode.NotFound);
            }
            unitOfWork.Products.Remove(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        public async Task<ServiceResult<IEnumerable<ProductDto?>>> GetAsync(CancellationToken cancellationToken)
        {
            var products = await unitOfWork.Products.GetWithCategoryAndStockAsync(cancellationToken);
            var mappedProducts = products?.Adapt<IEnumerable<ProductDto?>>();
            return ServiceResult<IEnumerable<ProductDto?>>.SuccessAsOk(mappedProducts);
        }

        public async Task<ServiceResult<ProductDetailDto?>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetByIdWithCategoryAndStockAsync(id, cancellationToken);
            var mappedProduct = product?.Adapt<ProductDetailDto?>();
            return ServiceResult<ProductDetailDto?>.SuccessAsOk(mappedProduct);
        }

        public async Task<ServiceResult<PagedResult<ProductDto?>>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.Products.GetPagedWithCategoryAsync(pageNumber, pageSize, cancellationToken);
            var mappedProducts = products.Data?.Adapt<IEnumerable<ProductDto?>>();
            PagedResult<ProductDto?> pagedResult = new(mappedProducts, products.TotalCount, pageNumber, pageSize);
            return ServiceResult<PagedResult<ProductDto?>>.SuccessAsOk(pagedResult);
        }

        public async Task<ServiceResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Products.GetByIdAsync(id, cancellationToken);
            if (product is null)
            {
                return ServiceResult<ProductDto>.Error("Ürün bulunamadı", HttpStatusCode.NotFound);
            }
            var hasAnyProduct = await unitOfWork.Products.AnyAsync(x => x.Name == dto.Name && x.Id != id, cancellationToken);
            if (hasAnyProduct)
            {
                return ServiceResult<ProductDto>.Error("Ürün adı kullanılıyor", HttpStatusCode.BadRequest);
            }
            var hasAnyCategory = await unitOfWork.Categories.AnyAsync(x => x.Id == dto.CategoryId, cancellationToken);
            if (!hasAnyCategory)
            {
                return ServiceResult<ProductDto>.Error("Kategori bulunamadı", HttpStatusCode.BadRequest);
            }
            var updateProduct = dto.Adapt(product);
            unitOfWork.Products.Update(updateProduct);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedProduct = await unitOfWork.Products.GetByIdWithCategoryAndStockAsync(id, cancellationToken);
            var mappedProduct = updatedProduct.Adapt<ProductDto>();
            return ServiceResult<ProductDto>.SuccessAsOk(mappedProduct);
        }
    }
}