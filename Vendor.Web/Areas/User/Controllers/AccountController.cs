using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Account;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region Ctor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region change password

        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePass)
        {
            if (ModelState.IsValid)
            {
                bool res = await _userService.ChangeUserPassword(changePass, User.GetUserId());
                if (res)
                {
                    TempData[SuccessMessage] = "کلمه عبور شما با موفقیت تغییر یافت";
                    TempData[InfoMessage] = "لطفا جهت تکمیل فرایند تغییر کلمه عبور ,مجددا وارد سایت شوید";
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Acount");
                }
                ModelState.AddModelError("NewPassword", "کلمه عبور جدید باید متفاوت با کلمه عبور فعلی باشد!");
            }
            return View(changePass);
        }

        #endregion

        #region edit profile

        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var editProfile = await _userService.GetProfileForEdit(User.GetUserId());
            if (editProfile == null) return NotFound();
            return View(editProfile);
        }

        [HttpPost("edit-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileDTO editeProfile, IFormFile avatarImage)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditProfileUser(editeProfile, User.GetUserId(), avatarImage);
                switch (result)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "حساب کاربری شما بلاک شده است!!";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما غیرفعال است!";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "حساب کاربری شما یافت نشد!";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = $" {editeProfile.FisrtName} {editeProfile.LastName}  عزیز اطلاعات شما با موفقیت ویرایش شد ";
                        return RedirectToAction("EditProfile");
                }
            }
            return View(editeProfile);
        }
        #endregion

    }
}
