using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.DTOs.Contacts;

namespace Vendor.Application.Services.interfaces
{
    public interface IContactService: IAsyncDisposable
    {
        #region contact us

        Task CreateContactUs(CreateContactUsDTO contactUs, string userIp, long? userId);

        #endregion

        #region ticket

        Task<AddTicketResult> AddUserTicket(AddTicketDTO addTicket, long userId);
        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filterTicket);
        Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId);
        Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answerTicket, long userId);
        #endregion
    }
}
