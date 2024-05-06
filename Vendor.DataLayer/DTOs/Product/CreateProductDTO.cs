using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Product;

namespace Vendor.DataLayer.DTOs.Product
{
	public class CreateProductDTO
	{
		[DisplayName("نام محصول")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
		public string Title { get; set; }

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

        public List<CreateProductColorDTO> ProductColors { get; set; }

        public List<CreateProductFeatureDTO> ProductFeatures { get; set; }

        public List<long> SelectedCategories { get; set; }
	}

    public enum CreateProductResult
    {
		Success,
		Error,
		NoImage
    }
}
