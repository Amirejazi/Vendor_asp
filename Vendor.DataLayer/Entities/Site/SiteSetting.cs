using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Site
{
    public class SiteSetting : BaseEntity
    {
        #region Properties

        [DisplayName("موبایل")]
        public string? Mobile { get; set; }

        [DisplayName("تلفن")]
        public string? Phone { get; set; }

        [DisplayName("ایمیل")]
        public string? Email { get; set; }

        [DisplayName("متن فوتر")]
        public string? FooterText { get; set; }

        [DisplayName("متن کپی رایت")]
        public string? CopyRight { get; set; }

        [DisplayName("آدرس نقشه")]
        public string? MapScript { get; set; }

        [DisplayName("آدرس")]
        public string? Address { get; set; }

        [DisplayName("درباره ما")]
        public string? AboutUs { get; set; }

        [DisplayName("اصلی هست / نیست")]
        public bool IsDefault { get; set; }

        #endregion
    }
}
