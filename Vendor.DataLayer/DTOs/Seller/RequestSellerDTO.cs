using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Vendor.DataLayer.DTOs.Seller
{
    public class RequestSellerDTO
    {
        [DisplayName("نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string StoreName { get; set; }

        [DisplayName("تلفن فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Phone { get; set; }

        [DisplayName("آدرس فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Address { get; set; }
    }

    public enum RequestSellerResult
    {
        Success,
        HasUnderProgressRequest,
        HasNotPermission
    }
}
