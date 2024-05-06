using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Vendor.DataLayer.DTOs.Product
{
    public class CreateOrEditProductGalleryDTO
    {
        [DisplayName("الویت نمایش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DispalyPriority { get; set; }

        [DisplayName("تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile Image { get; set; }

        public string? ImageName { get; set; }
    }

    public enum CreateOrEditProductGalleryResult
    {
        Success,
        NoForUser,
        NotFound,
        NoImage
    }
}
