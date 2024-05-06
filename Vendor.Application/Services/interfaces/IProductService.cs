using Microsoft.AspNetCore.Http;
using Vendor.DataLayer.DTOs.Common;
using Vendor.DataLayer.DTOs.Product;
using Vendor.DataLayer.Entities.Product;

namespace Vendor.Application.Services.interfaces
{
    public interface IProductService: IAsyncDisposable
    {
        #region product

        Task<FilterProductDTO> FilterProducts(FilterProductDTO filterProduct);
        Task<CreateProductResult> CreateProduct(CreateProductDTO createProduct, long sellerId, IFormFile productImage);
        Task<bool> AcceptProduct(long id);
        Task<bool> RejectProduct(RejectItemDTO rejectItem);
        Task<EditProductDTO> GetProductForEdit(long productId);
        Task<EditProductResult> EditProduct(EditProductDTO editProduct,long userId, IFormFile productImage);
        Task RemoveAllProductSelectedCategory(long productId);
        Task RemoveAllProductSelectedColor(long productId);
        Task AddProductSelectedCategory(long productId, List<long> selectedCategories);
        Task AddProductSelectedColor(long productId, List<CreateProductColorDTO> colors);
        Task<Product?> GetProductBySellerOwnerId(long productId, long userId);
        Task<ProductDetailDTO> GetProdcutDetailById(long productId);

        #endregion

        #region product gallery

        Task<List<ProductGallery>> GetAllProductGallery(long productId);
        Task<List<ProductGallery>> GetAllProductGalleryInSellerPanel(long productId, long userId);
        Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDTO gallery, long productId, long userId);
        Task<CreateOrEditProductGalleryDTO?> GetProductGalleryForEdit(long galleryId, long userId);
        Task<CreateOrEditProductGalleryResult> EditProductGallery(CreateOrEditProductGalleryDTO gallery, long galleryId, long userId);
        #endregion

        #region product categories

        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategory();

        #endregion

        #region product feature

        Task CreateProductFeature(long productId, List<CreateProductFeatureDTO> features);
        Task RemoveAllProductFeatures(long productId);

        #endregion
    }
}
