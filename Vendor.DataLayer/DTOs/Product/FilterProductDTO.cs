using System.ComponentModel.DataAnnotations;
using Vendor.DataLayer.DTOs.Paging;
using Vendor.DataLayer.DTOs.Seller;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.DTOs.Product
{
    public class FilterProductDTO: BasePaging
    {
        public FilterProductDTO()
        {
            Orderby = FilterProductOrderby.CreateDate_Des;
        }
        #region properties

        public string? Title { get; set; }

        public string? Category { get; set; }

        public long? SellerId { get; set; }

        public int FilterMinPrice { get; set; }

        public int FilterMaxPrice { get; set; }

        public int SelectedMinPrice { get; set; }

        public int SelectedMaxPrice { get; set; }

        public int PriceStep { get; set; } = 10000;

        public List<Entities.Product.Product?> Products { get; set; }

        public List<long> SelectedProductCategories { get; set; }

        public FilterProductState FilterProductState { get; set; }

        public FilterProductOrderby Orderby { get; set; }
        #endregion

        #region methode

        public FilterProductDTO SetProducts(List<Entities.Product.Product?> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.HowManyShowAfterAndBefore = paging.HowManyShowAfterAndBefore;

            return this;
        }

        #endregion
    }

    public enum FilterProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیرفعال")]
        NotActive
    }

    public enum FilterProductOrderby
    {
        [Display(Name = "تاریخ (نزولی)")]
        CreateDate_Des,
        [Display(Name = "تاریخ (صعودی)")]
        CreateDate_Asc,
        [Display(Name = "قیمت (نزولی)")]
        Price_Des,
        [Display(Name = "قیمت (صعودی)")]
        Price_Asc,
    }
}
