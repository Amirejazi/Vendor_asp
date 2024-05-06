using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Product
{
    public class ProductSelectedCategory: BaseEntity
    {
        #region properties

        public long ProductCategoryId { get; set; }

        public long ProductId { get; set; }

        #endregion

        #region relations

        public ProductCategory ProductCategory { get; set; }

        public Product Product { get; set; }


        #endregion
    }
}
