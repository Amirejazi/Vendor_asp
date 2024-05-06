using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Common;
using Vendor.DataLayer.DTOs.Product;
using Vendor.Web.Http;

namespace Vendor.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseContoller
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
        public async Task<IActionResult> Index(FilterProductDTO filterProduct)
        {
            return View(await _productService.FilterProducts(filterProduct));
        }

        #endregion

        #region accept product

        [HttpGet("accept-product")]
        public async Task<IActionResult> AcceptProduct(long id)
        {
            bool res = await _productService.AcceptProduct(id);
            if (res)
            {
                return JsonResultStatus.SendStatus(
                    JsonStatusType.Success,
                    message: "محصول مورد نظر باموفقیت تایید شد",
                    null);
            }
            else
            {
                return JsonResultStatus.SendStatus(
                    JsonStatusType.Danger,
                    message: "اطلاعاتی با این مشخصات یافت نشد!",
                    null);
            }
        }

        #endregion

        #region reject seller request

        [HttpPost("reject-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectProduct(RejectItemDTO rejectItem)
        {
            var res = await _productService.RejectProduct(rejectItem);
            if (res)
            {
                return JsonResultStatus.SendStatus(
                    JsonStatusType.Success,
                    message: "محصول مورد نظرباموفقیت رد شد",
                    rejectItem.Id);
            }
            else
            {
                return JsonResultStatus.SendStatus(
                    JsonStatusType.Danger,
                    message: "اطلاعاتی با این مشخصات یافت نشد!",
                    rejectItem.Id);
            }
        }

        #endregion
    }
}
