using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vendor.DataLayer.DTOs.Common
{
    public class RejectItemDTO
    {
        public long Id { get; set; }

        [DisplayName("توضیحات عدم اطلاعات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RejectMessage { get; set; }
    }
}
