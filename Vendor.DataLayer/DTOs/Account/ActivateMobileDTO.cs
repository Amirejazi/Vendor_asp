using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.DTOs.Site;

namespace Vendor.DataLayer.DTOs.Account
{
    public class ActivateMobileDTO: CaptchaViewModel
    {
        [DisplayName("تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Mobile { get; set; }

        [DisplayName("کد تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string MobileActiveCode { get; set; }
    }

    public enum ActivateMobileResult
    {
        Success,
        NotFound,
        NotMatch,
    }
}
