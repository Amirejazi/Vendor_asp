using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.ProductOrder
{
    public class Order: BaseEntity
    {
        #region properties

        public long UserId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public bool IsPaied { get; set; }

        [DisplayName("کد پیگیری")]
        [MaxLength(200, ErrorMessage ="{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string? TracingCode { get; set; }

        [DisplayName("توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string? Description { get; set; }
        #endregion

        #region relations

        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
