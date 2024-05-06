using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.implementions;
using Vendor.Application.Services.interfaces;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.ViewComponent
{
    #region site header

    public class SiteHeaderViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        #region Ctor

        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;

        public SiteHeaderViewComponent(ISiteService siteService, IUserService userService, IProductService productService, ISellerService sellerService)
        {
	        _siteService = siteService;
	        _userService = userService;
            _productService = productService;
            _sellerService = sellerService;
        }

        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSetting();
            ViewBag.user = null;
            if (User.Identity.IsAuthenticated)
            {
	            ViewBag.user = await _userService.GetUserByMobile(User.Identity.Name);
                ViewBag.HasUserAnyActivePanel = await _sellerService.HasUserAcitveSellerPanel(User.GetUserId());
            }

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategory();
            return View("SiteHeader");
        }
    }

    #endregion

    #region site footer

    public class SiteFooterViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        #region Ctor

        private readonly ISiteService _siteService;

        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SiteSetting = await _siteService.GetDefaultSiteSetting();
            return View("SiteFooter");
        }
    }

    #endregion

    #region home sliders

    public class HomeSliderViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        #region Ctor

        private readonly ISiteService _siteService;

        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _siteService.GetAllActiveSliders();
            return View("HomeSlider", sliders);
        }
    }

    #endregion


    #region user order

    public class UserOrderViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        #region Ctor

        private readonly IOrderService _orderService;

        public UserOrderViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // var openOrder = await _orderService.GetLatestOpenOrderOfUser(User.GetUserId());
            var openOrder = await _orderService.GetOpenUserDetail(User.GetUserId());
            return View("UserOrder", openOrder);
        }
    }

    #endregion
}
