using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Product
{
    public class ProductCategory : BaseEntity
    {
        #region properties

        public long? ParentId { get; set; }

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Title { get; set; }

        [DisplayName("عنوان در url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string UrlName { get; set; }

        [DisplayName("فعال/غیرفعال")]
        public bool IsActive { get; set; }

        #endregion

        #region relations

        public ProductCategory? Parent { get; set; }

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        #endregion
    }
}
