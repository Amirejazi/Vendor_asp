using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.DTOs.Paging;
using Vendor.DataLayer.Entities.Contacts;

namespace Vendor.DataLayer.DTOs.Contacts
{
    public class FilterTicketDTO: BasePaging
    {
        #region properties

        public long? UserId { get; set; }

        public string Title { get; set; }

        public FilterTicketState FilterTicketState { get; set; }

        public TicketSection? TicketSection { get; set; }

        public TicketPriority? TicketPriority { get; set; }

        public FilterTicketOrder OrderBy { get; set; }

        public List<Ticket> Tickets { get; set; }

        #endregion

        #region methods

        public FilterTicketDTO SetTickets(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.PageCount = paging.PageCount;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.HowManyShowAfterAndBefore = paging.HowManyShowAfterAndBefore;

            return this;
        }

        #endregion
    }

    public enum FilterTicketState
    {
        All,
        NotDeleted,
        Deleted
    }

    public enum FilterTicketOrder
    {
        CreateDate_DESC,
        CreateDate_ASC
    }
}
