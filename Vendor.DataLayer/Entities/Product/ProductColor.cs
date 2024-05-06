using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;
using Vendor.DataLayer.Entities.ProductOrder;

namespace Vendor.DataLayer.Entities.Product
{
	public class ProductColor: BaseEntity
	{
		#region properties

		public long ProductId { get; set; }

		[DisplayName("رنگ")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
		public string ColorName { get; set; }

		public int Price { get; set; }

        [DisplayName("کد رنگ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ColorCode { get; set; }

        #endregion

        #region relations

        public Product Product { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
