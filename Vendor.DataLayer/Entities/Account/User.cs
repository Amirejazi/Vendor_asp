using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Vendor.DataLayer.Entities.Common;
using Vendor.DataLayer.Entities.Contacts;
using Vendor.DataLayer.Entities.Store;

namespace Vendor.DataLayer.Entities.Account
{
    public class User: BaseEntity
    {
        #region Peroperties

        [DisplayName("ایمیل")]
        [MaxLength(200, ErrorMessage ="{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد!")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")] 
        public string EmailActiveCode { get; set; }

        [DisplayName("ایمیل فعال / غیر فعال")]
        public bool IsEmailActive { get; set; }

        [DisplayName("تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string MobileActiveCode { get; set; }

        [DisplayName("موبایل فعال / غیر فعال")]
        public bool IsMobileActive { get; set; }

        [DisplayName("نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string FisrtName { get; set; }

        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string LastName { get; set; }

        [DisplayName("تصویر آواتار")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string? Avatar { get; set; }

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Password { get; set; }

        [DisplayName("بلاک شده / نشده")]
        public bool IsBlocked { get; set; }

        #endregion

        #region Relations

        public ICollection<ContactUs> ContactUssCollection { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<TicketMessage> TicketMessage { get; set; }
        public ICollection<Seller> Sellers { get; set; }

        #endregion
    }
}
