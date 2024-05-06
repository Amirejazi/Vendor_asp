using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Vendor.DataLayer.Entities.Common;
using Vendor.DataLayer.Entities.Store;
using Vendor.DataLayer.Entities.ProductOrder;

namespace Vendor.DataLayer.Entities.Product
{
    public class Product : BaseEntity
    {
        #region properties

        public long ProductCategoryId { get; set; }

        public long SellerId { get; set; }

        [DisplayName("نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Title { get; set; }

        [DisplayName("تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ImageName { get; set; }

        [DisplayName("قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [DisplayName("توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ShortDescription { get; set; }

        [DisplayName("توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [DisplayName("فعال/غیر فعال")]
        public bool IsActive { get; set; }

        [DisplayName("وضعیت")]
        public ProductAcceptanceState ProductAcceptanceState { get; set; }

        [DisplayName("پیام تایید / عدم تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductAcceptOrRejectDescription { get; set; }

        #endregion

        #region relations

        public Seller Seller { get; set; }

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        public ICollection<ProductColor> ProductColors { get; set; }

        public ICollection<ProductGallery> ProductGalleries { get; set; }

        public ICollection<ProductFeature> ProductFeatures { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }

    public enum ProductAcceptanceState
    {
	    [Display(Name="درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
		Accepted,
		[Display(Name = "رد شده")]
		Rejected
    }
}
