using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.implementions;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Seller;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region Ctor

        private readonly IUserService _userService;
        private readonly ISellerService _sellerService;

        public SellerController(IUserService userService, ISellerService sellerService)
        {
            _userService = userService;
            _sellerService = sellerService;
        }

        #endregion

        #region request seller

        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }

        [HttpPost("request-seller-panel"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO requestSeller)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.AddNewSellerRequest(requestSeller, User.GetUserId());
                switch (res)
                {
                    case RequestSellerResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما دسترسی لازم رو جهت انجام فرایند مورد نظر را ندارید!";
                        break;
                    case RequestSellerResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "در خواست های قبلی شما در حال پیگیری هستند";
                        TempData[InfoMessage] = "در حال حاضر شما نمیتوانید درخواست جدیدی ثبت کنید";
                        break;
                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات شما در حال پیگیری می باشد";
                        return RedirectToAction("SellerRequests");
                        break;
                }
            }
            return View(requestSeller);
        }

        #endregion

        #region list seller request

        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filterSeller)
        {
            filterSeller.TakeEntity = 1;
            filterSeller.UserId = User.GetUserId();
            return View(await _sellerService.GetFilterSellers(filterSeller));
        }

        #endregion

        #region edit request seller

        [HttpGet("edit-request-seller/{id}")]
        public async Task<IActionResult> EditRequestSeller(long id)
        {
            var requestSeller = await _sellerService.GetRequestSellerForEdit(id, User.GetUserId());
            if (requestSeller == null) return NotFound();

            return View(requestSeller);
        }

        [HttpPost("edit-request-seller/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequestSeller(EditRequestSellerDTO editRequest)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.EditRequestSeller(editRequest, User.GetUserId());
                switch (res)
                {
                    case EditRequestSellerResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد!";
                        break;
                    case EditRequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ویرایش شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات از سر گرفته شد";
                        return RedirectToAction("SellerRequests");
                }
            }
            return View(editRequest);
        }

        #endregion
    }
}
