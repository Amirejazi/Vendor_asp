using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.implementions;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Product;
using Vendor.DataLayer.Entities.Product;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.Areas.Seller.Controllers
{

	public class ProductController : SellerBaseController
	{
		#region Ctor

		private readonly IProductService _productService;
		private readonly ISellerService _sellerService;

		public ProductController(IProductService productService, ISellerService sellerService)
        {
            _productService = productService;
            _sellerService = sellerService;
        }

        #endregion

        #region product

        #region list products

        [HttpGet("products/list")]
        public async Task<IActionResult> Index(FilterProductDTO filterProduct)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            filterProduct.SellerId = seller.Id;
            filterProduct.FilterProductState = FilterProductState.All;
            return View(await _productService.FilterProducts(filterProduct));
        }

        #endregion

        #region create product

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _productService.GetAllActiveProductCategory();
            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO createProduct, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var res = await _productService.CreateProduct(createProduct, seller.Id, productImage);
                switch (res)
                {
                    case CreateProductResult.NoImage:
                        TempData[WarningMessage] = "لطفا تصویر محصول خود را وارد کنید!";
                        break;
                    case CreateProductResult.Error:
                        TempData[WarningMessage] = "عملیات ثبت محصول با خطا مواجه شد!";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = $"محصول شما با نام {createProduct.Title} ثبت شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = await _productService.GetAllActiveProductCategory();
            return View(createProduct);
        }

        #endregion

        #region edit product

        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);
            if (product == null) return NotFound();

            ViewBag.Categories = await _productService.GetAllActiveProductCategory();
            return View(product);
        }

        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDTO editProduct, IFormFile? productImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProduct(editProduct, User.GetUserId(), productImage);
                switch (res)
                {
                    case EditProductResult.NotForUser:
                        TempData[ErrorMessage] = "در ویرایش اطلاعات خطایی رخ داده است!";
                        break;
                    case EditProductResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات وارد شده اشتباه است!";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "محصول شما با موفقیت ویرایش شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = await _productService.GetAllActiveProductCategory();
            return View(editProduct);
        }

        #endregion

        #endregion

        #region product gallery

        #region list

        [HttpGet("product-galleries/{productId}")]
        public async Task<IActionResult> GetProductGalleries(long productId)
        {
            ViewBag.productId = productId;
            return View(await _productService.GetAllProductGalleryInSellerPanel(productId, User.GetUserId()));
        }

        #endregion

        #region create gallery

        [HttpGet("create-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId)
        {
	        var product = await _productService.GetProductBySellerOwnerId(productId, User.GetUserId());
	        if (product == null) return NotFound();
            ViewBag.product = product;
	        return View();
        }

        [HttpPost("create-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId, CreateOrEditProductGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateProductGallery(gallery, productId, User.GetUserId());
                switch (res)
                {
                    case CreateOrEditProductGalleryResult.NoForUser:
                        TempData[ErrorMessage] = "لطفا شلوغ کاری نکنید:)";
                        break;
                    case CreateOrEditProductGalleryResult.NoImage:
                        TempData[WarningMessage] = "عکس مربوطه معتبر نمی باشد";
                        break;
                    case CreateOrEditProductGalleryResult.NotFound:
                        TempData[WarningMessage] = "تصویر مربوطه یافت نشد!";
                        break;
                    case CreateOrEditProductGalleryResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت گالری باموفقیت انجام شد";
                        return RedirectToAction("GetProductGalleries", new { productId = productId });
                }
            }
            return View(gallery);
        }

        #endregion

        #region edit gallery

        [HttpGet("product_{productId}/edit-gallery/{galleryId}")]
        public async Task<IActionResult> EditProductGallery(long productId, long galleryId)
        {
            var gallery = await _productService.GetProductGalleryForEdit(galleryId, User.GetUserId());
            if (gallery == null) return NotFound();

            return View(gallery);
        }

        [HttpPost("product_{productId}/edit-gallery/{galleryId}")]
        public async Task<IActionResult> EditProductGallery(long productId, long galleryId, CreateOrEditProductGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProductGallery(gallery, galleryId, User.GetUserId());
                switch (res)
                {
                    case CreateOrEditProductGalleryResult.NoForUser:
                        TempData[ErrorMessage] = "لطفا شلوغ کاری نکنید:)";
                        break;
                    case CreateOrEditProductGalleryResult.NotFound:
                        TempData[WarningMessage] = "تصویر مربوطه یافت نشد!";
                        break;
                    case CreateOrEditProductGalleryResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش گالری باموفقیت انجام شد";
                        return RedirectToAction("GetProductGalleries", new { productId = productId });
                }
            }
            return View(gallery);
        }

        #endregion
        #endregion
    }
}
