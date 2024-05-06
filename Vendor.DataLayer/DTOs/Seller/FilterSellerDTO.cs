using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.DTOs.Contacts;
using Vendor.DataLayer.DTOs.Paging;
using Vendor.DataLayer.Entities.Contacts;

namespace Vendor.DataLayer.DTOs.Seller
{
    public class FilterSellerDTO : BasePaging
    {
        #region properties

        public List<Entities.Store.Seller> Sellers { get; set; }

        public long? UserId { get; set; }

        public string? StoreName { get; set; }

        public string? Phone { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public FilterSellerState State { get; set; }
        #endregion

        #region methods

        public FilterSellerDTO SetSellers(List<Entities.Store.Seller> sellers)
        {
            this.Sellers = sellers;
            return this;
        }

        public FilterSellerDTO SetPaging(BasePaging paging)
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

    public enum FilterSellerState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
