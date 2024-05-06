using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Contacts;

namespace Vendor.DataLayer.DTOs.Contacts
{
    public class AddTicketDTO
    {
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Title { get; set; }

        [DisplayName("بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [DisplayName("اولویت")]
        public TicketPriority TicketPriority { get; set; }

        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
    }

    public enum AddTicketResult
    {
        Success,
        Error
    }
}
