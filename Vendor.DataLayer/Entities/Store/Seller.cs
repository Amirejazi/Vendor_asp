using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Store
{
    public class Seller : BaseEntity
    {
        #region properties

        public long UserId { get; set; }

        [DisplayName("نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string StoreName { get; set; }

        [DisplayName("تلفن فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Phone { get; set; }

        [DisplayName("تلفن همراه")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string? Mobile { get; set; }

        [DisplayName("آدرس فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Address { get; set; }

        [DisplayName("توضیحات فروشگاه")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string? Description { get; set; }

        [DisplayName("یادداشت های ادمین")]
        public string? AdminDescription { get; set; }

        [DisplayName("توضیحات (تایید / عدم تایید) اطلاعات")]
        public string? StoreAcceptanceDescription { get; set; }

        public StoreAcceptanceState StoreAcceptanceState { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        #endregion
    }

    public enum StoreAcceptanceState
    {
        [Display(Name="درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
