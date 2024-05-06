using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Contacts;
using Vendor.DataLayer.Entities.Site;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactService _contactService;
        private readonly ISiteService _siteService;
        private readonly ICaptchaValidator _captchaValidator;

        public HomeController(ILogger<HomeController> logger, IContactService contactService, ICaptchaValidator captchaValidator, ISiteService siteService)
        {
            _logger = logger;
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _siteService = siteService;
        }

        #region index

        public async Task<IActionResult> Index()
        {
            ViewBag.Banners = await _siteService.GetAllActiveBannersByPalcement(new List<BannerPlacement>
            {
                BannerPlacement.Home_1,
                BannerPlacement.Home_2,
                BannerPlacement.Home_3,
            });
            return View();
        }

        #endregion

        #region contact us

        [HttpGet("contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO contactUs)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contactUs.Captcha))
            {
                TempData[ErrorMessage] = "کپچای شما تایید نشد!";
                return View(contactUs);
            }

            if (ModelState.IsValid) 
            {
                await _contactService.CreateContactUs(contactUs, HttpContext.GetUserIp(), User.GetUserId());
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("ContactUs");
            }
            return View(contactUs);
        }

        #endregion

        #region about us

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var siteSetting = await _siteService.GetDefaultSiteSetting();
            return View(siteSetting);
        }

        #endregion

    }
}