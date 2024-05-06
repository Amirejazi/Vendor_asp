using System.Security.Claims;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Account;

namespace Vendor.Web.Controllers
{
    public class AcountController : SiteBaseController
    {
        #region Ctor

        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;

        public AcountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region register

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO registerUser)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(registerUser.Captcha))
            {
                TempData[ErrorMessage] = "کپچای شما تایید نشد!";
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View(registerUser);
            }

            var res = await _userService.RegisterUser(registerUser);
            switch (res)
            {
                case RegisterUserResult.MobileExists:
                    ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری میباشد!");
                    TempData[ErrorMessage] = "تلفن همراه وارد شده تکراری میباشد!";
                    break;
                case RegisterUserResult.Success:
                    TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد:)";
                    TempData[InfoMessage] = "کد تایید تلفن همراه برای شما ارسال شد";
                    return RedirectToAction("ActivateMobile", "Acount", new { mobileNumber = registerUser.Mobile });
            }
            return View(registerUser);
        }
        #endregion

        #region activate mobile

        [HttpGet("activate-mobile/{mobileNumber}")]
        public IActionResult ActivateMobile(string mobileNumber)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activateMobileDTO = new ActivateMobileDTO { Mobile = mobileNumber };
            return View(activateMobileDTO);
        }

        [HttpPost("activate-mobile/{mobileNumber}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateMobile(ActivateMobileDTO activateMobile)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(activateMobile.Captcha))
            {
                TempData[ErrorMessage] = "کپچای شما تایید نشد!";
                return View();
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.ActivateMobile(activateMobile);
                switch (res)
                {
                    case ActivateMobileResult.NotFound:
                        TempData[ErrorMessage] = "!کابری با این مشخصات یافت نشد";
                        break;
                    case ActivateMobileResult.NotMatch:
                        TempData[WarningMessage] = "کد تایید نادرست می باشد!";
                        break;
                    case ActivateMobileResult.Success:
                        TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد";
                        return RedirectToAction("Login");
                }
            }
            return View(activateMobile);
        }
        #endregion

        #region login

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(loginUser.Captcha))
            {
                TempData[ErrorMessage] = "کپچای شما تایید نشد!";
                return View();
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.GetUserForLogin(loginUser);
                switch (res)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد!";
                        break;
                    case LoginUserResult.NotActivated:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نشده است!";
                        break;
                    case LoginUserResult.Success:
                        var user = await _userService.GetUserByMobile(loginUser.Mobile);
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, user.Mobile),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity);

                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = loginUser.RememberMe
                        };

                        await HttpContext.SignInAsync(principal, properties);
                        TempData[SuccessMessage] = "ورود شما با موفقیت انجام شد";
                        return Redirect("/");

                        break;
                }
            }
            return View();
        }
        #endregion

        #region forgot password

        [HttpGet("forgot-pass")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-pass"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کپچای شما تایید نشد!";
                return View();
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.RecoveryUserPassword(forgot);
                switch (res)
                {
                    case ForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد!";
                        break;
                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه عبور جدید برای شما پیامک شد";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری کلمه عبور خود را تغییر دهید";
                        return Redirect("/login");
                }
            }

            return View();
        }

        #endregion

        #region logout

        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion
    }
}
