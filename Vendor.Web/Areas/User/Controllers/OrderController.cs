using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.implementions;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Order;
using Vendor.Web.Http;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.Areas.User.Controllers
{
    public class OrderController : UserBaseController
    {
        #region Ctor

        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        #endregion

        #region add product to open order

        [AllowAnonymous]
        [HttpPost("add-product-to-order")]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderDTO order)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _orderService.AddProductToOpenOrder(User.GetUserId(), order);
                    return JsonResultStatus.SendStatus(
                        JsonStatusType.Success,
                        message: "محصول مورد نظرباموفقیت به سبد خرید اضافه شد",
                        null);
                }
                else
                {
                    return JsonResultStatus.SendStatus(
                        JsonStatusType.Danger,
                        message: "برای خرید محصول مورد نظر ابتدا باید وارد شوید",
                        null);

                }
            }
            return JsonResultStatus.SendStatus(
                JsonStatusType.Danger,
                message: "اطلاعاتی با این مشخصات یافت نشد!",
                null);
        }

        #endregion

        #region open order

        [HttpGet("open-order")]
        public async Task<IActionResult> UserOpenOrder()
        {
            var openOrder = await _orderService.GetOpenUserDetail(User.GetUserId());
            return View(openOrder);
        }

        #endregion
    }
}
