using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Contacts;
using Vendor.DataLayer.DTOs.Paging;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Entities.Contacts;
using Vendor.DataLayer.Repository;

namespace Vendor.Application.Services.implementions
{
    public class ContactService : IContactService
    {
        #region Ctor

        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;

        public ContactService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
            await _ticketRepository.DisposeAsync();
            await _ticketMessageRepository.DisposeAsync();
        }

        #endregion

        #region contact us

        public async Task CreateContactUs(CreateContactUsDTO contactUs, string userIp, long? userId)
        {
            var contactus = new ContactUs()
            {
                UserId = userId != null && userId.Value != 0 ? userId.Value : null,
                UserIp = userIp,
                FullName = contactUs.FullName,
                Email = contactUs.Email,
                Subject = contactUs.Subject,
                Text = contactUs.Text
            };
            await _contactUsRepository.AddEntity(contactus);
            await _contactUsRepository.SaveChanges();
        }


        #endregion

        #region ticket

        public async Task<AddTicketResult> AddUserTicket(AddTicketDTO addTicket, long userId)
        {
            if (string.IsNullOrEmpty(addTicket.Text)) return AddTicketResult.Error;

            // add ticket
            var newTicket = new Ticket
            {
                OwnerId = userId,
                Title = addTicket.Title,
                TicketPriority = addTicket.TicketPriority,
                TicketSection = addTicket.TicketSection,
                TicketState = TicketState.UnderProgress,
                IsReadByOwner = true
            };

            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();

            //add ticketMessage
            var newTicketMessag = new TicketMessage
            {
                SenderId = newTicket.OwnerId,
                TicketId = newTicket.Id,
                Text = addTicket.Text
            };
            await _ticketMessageRepository.AddEntity(newTicketMessag);
            await _ticketMessageRepository.SaveChanges();
            return AddTicketResult.Success;
        }

        public async Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filterTicket)
        {
            var query = _ticketRepository.GetQuery();

            #region state

            switch (filterTicket.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(t => t.IsDelete);
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(t => !t.IsDelete);
                    break;
            }

            switch (filterTicket.OrderBy)
            {
                case FilterTicketOrder.CreateDate_ASC:
                    query = query.OrderBy(t => t.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_DESC:
                    query = query.OrderByDescending(t => t.CreateDate);
                    break;
            }

            #endregion

            #region filter

            if (filterTicket.TicketSection != null)
                query = query.Where(t => t.TicketSection == filterTicket.TicketSection.Value);

            if (filterTicket.TicketPriority != null)
                query = query.Where(t => t.TicketPriority == filterTicket.TicketPriority.Value);

            if (filterTicket.UserId != null && filterTicket.UserId != 0)
                query = query.Where(t => t.OwnerId == filterTicket.UserId);

            if (string.IsNullOrEmpty(filterTicket.Title))
                query = query.Where(t => EF.Functions.Like(t.Title, $"%{filterTicket.Title}%"));

            #endregion

            #region paging

            var ticketCount = await query.CountAsync();
            var pager = Pager.Biuld(filterTicket.PageId, ticketCount, filterTicket.TakeEntity, filterTicket.HowManyShowAfterAndBefore);
            var allTicket = await query.Paging(pager).ToListAsync();

            #endregion

            filterTicket.SetPaging(pager).SetTickets(allTicket);

            return filterTicket;
        }

        public async Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().Include(t => t.Owner)
                .SingleOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null || ticket.OwnerId != userId) return null;

            return new TicketDetailDTO()
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetQuery()
                    .OrderByDescending(tm => tm.CreateDate)
                    .Where(tm => tm.TicketId == ticket.Id && !tm.IsDelete).ToListAsync()
            };
        }

        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answerTicket, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().SingleOrDefaultAsync(t => t.Id == answerTicket.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;
            if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;

            var ticketMeassge = new TicketMessage
            {
                SenderId = userId,
                TicketId = ticket.Id,
                Text = answerTicket.Text
            };
            await _ticketMessageRepository.AddEntity(ticketMeassge);
            await _contactUsRepository.SaveChanges();

            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;
            _ticketRepository.EditEntity(ticket);
            await _ticketRepository.SaveChanges();

            return AnswerTicketResult.Success;
        }

        #endregion
    }
}
