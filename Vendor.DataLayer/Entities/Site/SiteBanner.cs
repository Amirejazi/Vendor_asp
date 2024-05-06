
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Site
{
    public class SiteBanner: BaseEntity
    {
        [DisplayName("نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ImageName { get; set; }

        [DisplayName("آدرس ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Url { get; set; }

        [DisplayName("سایز(کلاس های نمایشی)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ColSize { get; set; }

        public BannerPlacement BannerPlacement { get; set; }

        [DisplayName("فعال/غیرفعال")]
        public bool IsActive { get; set; }
    }
}
