﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Common;
using Vendor.DataLayer.Entities.Product;

namespace Vendor.DataLayer.Entities.ProductOrder
{
    public class OrderDetail: BaseEntity
    {
        #region properties

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public long? ProductColorId { get; set; }

        public int Count { get; set; }

        public int ProductPrice { get; set; }

        public int ProductColorPrice { get; set; }

        #endregion

        #region relations

        public Order Order { get; set; }

        public Product.Product Product { get; set; }

        public ProductColor ProductColor { get; set; }

        #endregion
    }
}
