using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Product;

namespace Vendor.DataLayer.DTOs.Product
{
    public class ProductDetailDTO
    {
        public long ProductId { get; set; }

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

        public Entities.Store.Seller Seller { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        public List<ProductColor> ProductColors { get; set; }

        public List<ProductGallery> ProductGalleries { get; set; }

        public List<ProductFeature> ProductFeatures { get; set; }

        public List<Entities.Product.Product> RelatedProducts { get; set; }
    }
}
