using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Entities.Common;

namespace Vendor.DataLayer.Entities.Contacts
{
    public class TicketMessage: BaseEntity
    {
        #region properties

        public long TicketId { get; set; }

        public long SenderId { get; set; }

        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }

        #endregion

        #region relations

        public Ticket Ticket { get; set; }

        public User Sender { get; set; }

        #endregion
    }
}
