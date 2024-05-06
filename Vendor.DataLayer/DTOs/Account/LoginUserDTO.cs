using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Vendor.DataLayer.DTOs.Site;


namespace Vendor.DataLayer.DTOs.Account
{
    public class LoginUserDTO : CaptchaViewModel
    {
        [DisplayName("تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Mobile { get; set; }

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public enum LoginUserResult
    {
        Success,
        NotFound,
        NotActivated
    }
}
