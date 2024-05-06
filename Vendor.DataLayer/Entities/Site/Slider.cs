using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Site
{
    public class Slider : BaseEntity
    {
        #region propreties

        [DisplayName("عنوان اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string MainHeader { get; set; }

        [DisplayName("عنوان دوم")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string? SecondHeader { get; set; }

        [DisplayName("نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ImageName { get; set; }

        [DisplayName("توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Description { get; set; }

        [DisplayName("لینک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Link { get; set; }

        [DisplayName("فعال/غیرفعال")]
        public bool IsActive { get; set; }

        #endregion
    }
}
