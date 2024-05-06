using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Contacts
{
    public class Ticket : BaseEntity
    {
        #region properties

        public long OwnerId { get; set; }

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از{1} کارکتر باشد!")]
        public string Title { get; set; }

        [DisplayName("وضعیت تیکت")]
        public TicketState TicketState { get; set; }

        [DisplayName("بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [DisplayName("اولویت")]
        public TicketPriority TicketPriority { get; set; }


        public bool IsReadByOwner { get; set; }

        public bool IsReadByAdmin { get; set; }

        #endregion

        #region relations

        public User Owner { get; set; }

        public ICollection<TicketMessage> TicketMessage { get; set; }

        #endregion
    }

    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        Support,
        [Display(Name = "فنی")]
        Technical,
        [Display(Name = "آموزشی")]
        Academy,
    }

    public enum TicketPriority
    {
        [Display(Name = "پایین")]
        Low,
        [Display(Name = "متوسط")]
        Medium,
        [Display(Name = "زیاد")]
        Hight,
    }

    public enum TicketState
    {
        [Display(Name = "درحال بررسی")]
        UnderProgress,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }
}
