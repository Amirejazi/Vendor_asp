using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Vendor.DataLayer.DTOs.Site;

namespace Vendor.DataLayer.DTOs.Account
{
    public class ForgotPasswordDTO: CaptchaViewModel
    {
        [DisplayName("تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Mobile { get; set; }
    }

    public enum ForgotPasswordResult
    {
        Success,
        NotFound,
        Error
    }
}
