using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Product
{
    public class ProductGallery: BaseEntity
    {
        #region properties

        public long ProductId { get; set; }

        public int DisplayPeriority { get; set; }

        [DisplayName("نام تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string ImageName { get; set; }

        #endregion

        #region relations

        public Product Product { get; set; }

        #endregion
    }
}
