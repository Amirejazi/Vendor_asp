using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.implementions;
using Vendor.DataLayer.DTOs.Common;
using Vendor.DataLayer.DTOs.Seller;
using Vendor.Web.Http;

namespace Vendor.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseContoller
    {
        #region Ctor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region seller requests

        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filterSeller)
        {
            filterSeller.TakeEntity = 1;
            return View(await _sellerService.GetFilterSellers(filterSeller));
        }

        #endregion

        #region accept seller request

        [HttpGet("accept-request")]
        public async Task<IActionResult> AcceptSellerRequest(long requestId)
        {
            bool res = await _sellerService.AcceptSellerRequest(requestId);
            if (res)
            {
                return JsonResultStatus.SendStatus(
                    JsonStatusType.Success,
                    message: "درخواست مورد نظرباموفقیت تایید شد",
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

        [HttpPost("reject-request"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerRequest(RejectItemDTO rejectItem)
        {
            var res = await _sellerService.RejectSellerRequest(rejectItem);
            if (res)
            {
                return JsonResultStatus.SendStatus(
                    JsonStatusType.Success,
                    message: "درخواست مورد نظرباموفقیت رد شد",
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
