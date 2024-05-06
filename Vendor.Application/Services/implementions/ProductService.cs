using MarketPlace.Application.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.Services.interfaces;
using Vendor.Application.Utils;
using Vendor.DataLayer.DTOs.Common;
using Vendor.DataLayer.DTOs.Paging;
using Vendor.DataLayer.DTOs.Product;
using Vendor.DataLayer.Entities.Product;
using Vendor.DataLayer.Repository;

namespace Vendor.Application.Services.implementions
{
	public class ProductService : IProductService
	{
		#region Ctor

		private readonly IGenericRepository<Product?> _productRepository;
		private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
		private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;
		private readonly IGenericRepository<ProductColor> _productColorRepository;
		private readonly IGenericRepository<ProductGallery> _productGalleryRepository;
		private readonly IGenericRepository<ProductFeature> _productFeatureRepository;

		public ProductService(IGenericRepository<Product?> productRepository, IGenericRepository<ProductCategory> productCategoryRepository, IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository, IGenericRepository<ProductColor> productColorRepository, IGenericRepository<ProductGallery> productGalleryRepository, IGenericRepository<ProductFeature> productFeatureRepository)
		{
			_productRepository = productRepository;
			_productCategoryRepository = productCategoryRepository;
			_productSelectedCategoryRepository = productSelectedCategoryRepository;
            _productColorRepository = productColorRepository;
            _productGalleryRepository = productGalleryRepository;
            _productFeatureRepository = productFeatureRepository;
        }

		#endregion

		#region dispose

		public async ValueTask DisposeAsync()
		{
			await _productRepository.DisposeAsync();
			await _productCategoryRepository.DisposeAsync();
			await _productSelectedCategoryRepository.DisposeAsync();
			await _productColorRepository.DisposeAsync();
			await _productGalleryRepository.DisposeAsync();
			await _productFeatureRepository.DisposeAsync();
		}

		#endregion

		#region product

		public async Task<FilterProductDTO> FilterProducts(FilterProductDTO filterProduct)
		{
			var query = _productRepository.GetQuery().Include(p => p.ProductSelectedCategories).ThenInclude(p => p.ProductCategory).AsQueryable();

            filterProduct.FilterMaxPrice = await query.MaxAsync(p => p.Price);
            #region state

            switch (filterProduct.FilterProductState)
			{
				case FilterProductState.All:
					break;
				case FilterProductState.UnderProgress:
					query = query.Where(p => p.ProductAcceptanceState == ProductAcceptanceState.UnderProgress);
					break;
				case FilterProductState.Accepted:
					query = query.Where(p => p.IsActive && p.ProductAcceptanceState == ProductAcceptanceState.Accepted);
					break;
				case FilterProductState.Rejected:
					query = query.Where(p => p.ProductAcceptanceState == ProductAcceptanceState.Rejected);
					break;
				case FilterProductState.Active:
					query = query.Where(p => p.IsActive);
					break;
				case FilterProductState.NotActive:
					query = query.Where(p => !p.IsActive);
					break;
			}

            switch (filterProduct.Orderby)
            {
                case FilterProductOrderby.CreateDate_Des:
                    query = query.OrderByDescending(p => p.CreateDate);
                    break;
                case FilterProductOrderby.CreateDate_Asc:
                    query = query.OrderBy(p => p.CreateDate);
                    break;
                case FilterProductOrderby.Price_Des:
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case FilterProductOrderby.Price_Asc:
                    query = query.OrderBy(p => p.Price);
                    break;

            }
			#endregion

			#region filter

			if (string.IsNullOrEmpty(filterProduct.Title))
			{
				query = query.Where(p => EF.Functions.Like(p.Title, $"%{filterProduct.Title}%"));
			}

			if (filterProduct.SellerId != null && filterProduct.SellerId != 0)
			{
				query = query.Where(p => p.SellerId == filterProduct.SellerId);
			}


            if(filterProduct.SelectedMaxPrice == 0) filterProduct.SelectedMaxPrice = filterProduct.FilterMaxPrice;

            query = query.Where(p => p.Price <= filterProduct.SelectedMaxPrice);
            query = query.Where(p => p.Price >= filterProduct.SelectedMinPrice);

            if (!string.IsNullOrEmpty(filterProduct.Category))
            {
                query = query.Where(p =>
                    p.ProductSelectedCategories.Any(c => c.ProductCategory.UrlName == filterProduct.Category));
            }
            #endregion

            #region paging

            var entityCount = await query.CountAsync();
			var pager = Pager.Biuld(filterProduct.PageId, entityCount, filterProduct.TakeEntity, filterProduct.HowManyShowAfterAndBefore);
			var allEntity = await query.Paging(pager).ToListAsync();

			#endregion

			filterProduct.SetPaging(pager).SetProducts(allEntity);
			return filterProduct;
		}

        public async Task<CreateProductResult> CreateProduct(CreateProductDTO createProduct, long sellerId, IFormFile productImage)
        {
            if (productImage == null) return CreateProductResult.NoImage;

			var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
            var res = productImage.AddImageToServer(imageName, PathExtentions.ProductImageServer, 150, 150, PathExtentions.ProductThumbnailImageServer);

            if (res)
            {
                // create product
                var newProduct = new Product()
                {
                    Title = createProduct.Title,
                    Price = createProduct.Price,
                    ShortDescription = createProduct.ShortDescription,
                    Description = createProduct.Description,
                    IsActive = createProduct.IsActive,
                    SellerId = sellerId,
					ImageName = imageName,
					ProductAcceptanceState = ProductAcceptanceState.UnderProgress,
					ProductAcceptOrRejectDescription = ""
                };
                await _productRepository.AddEntity(newProduct);
                await _productRepository.SaveChanges();

                await AddProductSelectedCategory(newProduct.Id, createProduct.SelectedCategories);
                await _productCategoryRepository.SaveChanges();

                await AddProductSelectedColor(newProduct.Id, createProduct.ProductColors);
                await _productColorRepository.SaveChanges();

                await CreateProductFeature(newProduct.Id, createProduct.ProductFeatures);

                return CreateProductResult.Success;
            }

            return CreateProductResult.Error;
        }

        public async Task<bool> AcceptProduct(long id)
        {
            var product = await _productRepository.GetEntityById(id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Accepted;
                product.ProductAcceptOrRejectDescription = $"محصول مورد نظر در تاریخ {DateTime.Now.ToStringShamsiDate()} مورد تایید سایت قرار گرفت";
				_productRepository.EditEntity(product);
				await _productRepository.SaveChanges();
				return true;
            }
			return false;
        }

        public async Task<bool> RejectProduct(RejectItemDTO rejectItem)
        {
            var product = await _productRepository.GetEntityById(rejectItem.Id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Rejected;
                product.ProductAcceptOrRejectDescription = rejectItem.RejectMessage;
                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<EditProductDTO> GetProductForEdit(long productId)
        {
	        var product = await _productRepository.GetEntityById(productId);
	        if (product == null) return null;
	        return new EditProductDTO()
	        {
		        Id = product.Id,
				Title = product.Title,
				Description = product.Description,
				ShortDescription = product.ShortDescription,
				Price = product.Price,
				IsActive = product.IsActive,
				ImageName = product.ImageName,
				ProductColors = await _productColorRepository.GetQuery()
					.Where(s=>!s.IsDelete && s.ProductId==productId)
					.Select(s=>new CreateProductColorDTO{ColorName = s.ColorName, Price = s.Price, ColorCode = s.ColorCode})
					.ToListAsync(),
				SelectedCategories = await _productSelectedCategoryRepository.GetQuery().Where(s => s.ProductId==productId).Select(s=>s.ProductCategoryId).ToListAsync(),
                ProductFeatures = await _productFeatureRepository.GetQuery()
                    .Where(s => !s.IsDelete && s.ProductId == productId)
                    .Select(s => new CreateProductFeatureDTO() { FeatureTitle = s.FeatureTitle, FeatureValue = s.FeatureValue})
                    .ToListAsync(),
            };
        }

        public async Task<EditProductResult> EditProduct(EditProductDTO editProduct, long userId, IFormFile productImage)
        {
            var mainProduct = await _productRepository.GetQuery().Include(p => p.Seller).SingleOrDefaultAsync(p => p.Id == editProduct.Id);
            if (mainProduct == null) return EditProductResult.NotFound;
            if (mainProduct.Seller.UserId != userId) return EditProductResult.NotForUser;

            mainProduct.Title = editProduct.Title;
			mainProduct.Description = editProduct.Description;
			mainProduct.ShortDescription = editProduct.ShortDescription;
			mainProduct.Price = editProduct.Price;
			mainProduct.IsActive = editProduct.IsActive;
            mainProduct.ProductAcceptanceState = ProductAcceptanceState.UnderProgress;

            if (productImage != null)
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
                var res = productImage.AddImageToServer(imageName, PathExtentions.ProductImageServer, 150, 150, PathExtentions.ProductThumbnailImageServer, mainProduct.ImageName);
                if (res)
                {
                    mainProduct.ImageName = imageName;
                }
            }

            await RemoveAllProductSelectedCategory(mainProduct.Id);
            await AddProductSelectedCategory(mainProduct.Id, editProduct.SelectedCategories);
            await _productSelectedCategoryRepository.SaveChanges();

            await RemoveAllProductSelectedColor(mainProduct.Id);
            await AddProductSelectedColor(mainProduct.Id, editProduct.ProductColors);
            await _productColorRepository.SaveChanges();

            await RemoveAllProductFeatures(mainProduct.Id);
            await CreateProductFeature(mainProduct.Id, editProduct.ProductFeatures);

            return EditProductResult.Success;
        }

        public async Task RemoveAllProductSelectedCategory(long productId)
        {
            var oldProductCartegories = await _productSelectedCategoryRepository.GetQuery().Where(c => c.ProductId == productId).ToListAsync();
            _productSelectedCategoryRepository.DeletePermanentEntities(oldProductCartegories);
        }

        public async Task RemoveAllProductSelectedColor(long productId)
        {
            var oldProductColors = await _productColorRepository.GetQuery().Where(c => c.ProductId == productId).ToListAsync();
            _productColorRepository.DeletePermanentEntities(oldProductColors);
        }

        public async Task AddProductSelectedCategory(long productId, List<long> selectedCategories)
        {
            var productSelectedCategories = new List<ProductSelectedCategory>();
            foreach (var categoryId in selectedCategories)
            {
                productSelectedCategories.Add(new ProductSelectedCategory
                {
                    ProductId = productId,
                    ProductCategoryId = categoryId
                });
            }

            await _productSelectedCategoryRepository.AddRangeEntities(productSelectedCategories);
        }

        public async Task AddProductSelectedColor(long productId, List<CreateProductColorDTO> colors)
        {
            var productColors = new List<ProductColor>();
            foreach (var productColor in colors)
            {
                if (!productColors.Any(s => s.ColorName == productColor.ColorName))
                {
                    productColors.Add(new ProductColor()
                    {
                        ColorName = productColor.ColorName,
                        Price = productColor.Price,
                        ColorCode = productColor.ColorCode,
                        ProductId = productId
                    });
                }
            }

            await _productColorRepository.AddRangeEntities(productColors);
        }

        public async Task<Product?> GetProductBySellerOwnerId(long productId, long userId)
        {
	        return await _productRepository.GetQuery().Include(p => p.Seller).Where(p => p.Id == productId && p.Seller.UserId == userId).SingleOrDefaultAsync();
        }

        public async Task<ProductDetailDTO> GetProdcutDetailById(long productId)
        {
            var product = await _productRepository.GetQuery()
                .Include(p => p.Seller)
                .Include(p => p.ProductSelectedCategories)
                .ThenInclude(p => p.ProductCategory)
                .Include(p => p.ProductGalleries)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductFeatures)
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null) return null;

            var selectedCategoryIds = product.ProductSelectedCategories.Select(s => s.ProductCategoryId).ToList();

            return new ProductDetailDTO()
            {
                ProductId = product.Id,
                Title = product.Title,
                Price = product.Price,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                ImageName = product.ImageName,
                Seller = product.Seller,
                ProductCategories = product.ProductSelectedCategories.Select(c => c.ProductCategory).ToList(),
                ProductColors = product.ProductColors.ToList(),
                ProductGalleries = product.ProductGalleries.ToList(),
                ProductFeatures = product.ProductFeatures.ToList(),
                RelatedProducts = await _productRepository.GetQuery()
                    .Include(p => p.ProductSelectedCategories)
                    .Where(p => p.ProductSelectedCategories.Any(s => selectedCategoryIds.Contains(s.ProductCategoryId)) && p.Id != product.Id && p.ProductAcceptanceState==ProductAcceptanceState.Accepted).ToListAsync()
            };
        }

        #endregion

        #region product gallery

        public async Task<List<ProductGallery>> GetAllProductGallery(long productId)
        {
            return await _productGalleryRepository.GetQuery().Where(pg => pg.ProductId == productId).ToListAsync();
        }

        public async Task<List<ProductGallery>> GetAllProductGalleryInSellerPanel(long productId, long userId)
        {
            return await _productGalleryRepository.GetQuery().Include(s => s.Product).ThenInclude(s => s.Seller)
                .Where(s => s.ProductId == productId && s.Product.Seller.UserId == userId).ToListAsync();
        }

        public async Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDTO gallery, long productId, long userId)
        {
            var product = await _productRepository.GetQuery().Include(s => s.Seller).SingleOrDefaultAsync(s => s.Id == productId);
            if (product == null) return CreateOrEditProductGalleryResult.NotFound;
            if (product.Seller.UserId != userId) return CreateOrEditProductGalleryResult.NoForUser;
            if (gallery.Image == null && !gallery.Image.IsImage()) return CreateOrEditProductGalleryResult.NoImage;

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(gallery.Image.FileName);
            gallery.Image.AddImageToServer(imageName, PathExtentions.ProductGalleryImageServer, 150, 150,
                PathExtentions.ProductGalleryThumbnailImageServer);

            await _productGalleryRepository.AddEntity(new ProductGallery()
            {
                ProductId = product.Id,
                DisplayPeriority = gallery.DispalyPriority,
                ImageName = imageName
            });
            await _productRepository.SaveChanges();
            return CreateOrEditProductGalleryResult.Success;
        }

        public async Task<CreateOrEditProductGalleryDTO?> GetProductGalleryForEdit(long galleryId, long userId)
        {
            var gallery = await _productGalleryRepository.GetQuery().Include(s => s.Product).ThenInclude(s => s.Seller)
                .Where(g =>g.Id == galleryId && g.Product.Seller.UserId == userId).SingleOrDefaultAsync();

            if (gallery == null) return null;

            return new CreateOrEditProductGalleryDTO
            {
                DispalyPriority = gallery.DisplayPeriority,
                ImageName = gallery.ImageName
            };
        }

        public async Task<CreateOrEditProductGalleryResult> EditProductGallery(CreateOrEditProductGalleryDTO galleryDTO, long galleryId, long userId)
        {
            var gallery = await _productGalleryRepository.GetQuery().Include(s => s.Product).ThenInclude(s => s.Seller)
                .Where(g => g.Id == galleryId && g.Product.Seller.UserId == userId).SingleOrDefaultAsync();

            if (gallery == null) return CreateOrEditProductGalleryResult.NotFound;
            if (gallery.Product.Seller.UserId != userId) return CreateOrEditProductGalleryResult.NoForUser;

            if (galleryDTO.Image != null && galleryDTO.Image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(galleryDTO.Image.FileName);
                var res =galleryDTO.Image.AddImageToServer(imageName, PathExtentions.ProductGalleryImageServer, 150, 150,
                    PathExtentions.ProductGalleryThumbnailImageServer, gallery.ImageName);

                gallery.ImageName = imageName;
            }

            gallery.DisplayPeriority = galleryDTO.DispalyPriority;

            _productGalleryRepository.EditEntity(gallery);
            await _productRepository.SaveChanges();

            return CreateOrEditProductGalleryResult.Success;
        }

        #endregion

        #region product categories

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long? parentId)
        {
            if (parentId != null || parentId != 0)
            {
                return await _productCategoryRepository.GetQuery().Where(c => !c.IsDelete && c.IsActive && c.ParentId == parentId).ToListAsync();
            }
            return await _productCategoryRepository.GetQuery().Where(c => !c.IsDelete && c.IsActive && c.ParentId == null).ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllActiveProductCategory()
        {
            return await _productCategoryRepository.GetQuery().Where(c => !c.IsDelete && c.IsActive).ToListAsync();
        }

        #endregion

        #region product feature

        public async Task CreateProductFeature(long productId, List<CreateProductFeatureDTO> features)
        {
            var newProductFeatures = new List<ProductFeature>();
            if (features != null && features.Any())
            {
                foreach (var feature in features)
                {
                    newProductFeatures.Add(new ProductFeature()
                    {
                        ProductId = productId,
                        FeatureTitle = feature.FeatureTitle,
                        FeatureValue = feature.FeatureValue,
                    });
                }

                await _productFeatureRepository.AddRangeEntities(newProductFeatures);
                await _productFeatureRepository.SaveChanges();
            }
        }

        public async Task RemoveAllProductFeatures(long productId)
        {
            var productFeatures = await _productFeatureRepository.GetQuery().Where(p => p.ProductId == productId).ToListAsync();
            _productFeatureRepository.DeletePermanentEntities(productFeatures);
            await _productFeatureRepository.SaveChanges();

        }

        #endregion
    }
}
