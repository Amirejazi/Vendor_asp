using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Product;

namespace Vendor.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region filter products

        [HttpGet("products")]
        [HttpGet("products/{Category}")]
        public async Task<IActionResult> FilterProducts(FilterProductDTO filter)
        {
            filter.TakeEntity = 9;
            filter.FilterProductState = FilterProductState.Accepted;
            filter = await _productService.FilterProducts(filter);
            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategory();
            if (filter.PageId > filter.GetLastPage() && filter.GetLastPage() != 0)
                return NotFound();

            return View(filter);
        }

        #endregion

        #region product details

        [HttpGet("products/{productId}/{title}")]
        public async Task<IActionResult> ProductDetail(long productId, string title)
        {
            var product = await _productService.GetProdcutDetailById(productId);
            if (product == null) return NotFound();

            return View(product);
        }

        #endregion
    }
}
