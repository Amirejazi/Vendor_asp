using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Contacts;
using Vendor.Web.Controllers;
using Vendor.Web.PresentationExtentions;

namespace Vendor.Web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {
        #region Ctor

        private readonly IContactService _contactService;

        public TicketController(IContactService contactService)
        {
            _contactService = contactService;
        }

        #endregion

        #region list

        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketDTO filterTicket)
        {
            filterTicket.UserId = User.GetUserId();
            filterTicket.FilterTicketState = FilterTicketState.NotDeleted;
            filterTicket.OrderBy = FilterTicketOrder.CreateDate_DESC;
            return View(await _contactService.FilterTickets(filterTicket));
        }

        #endregion

        #region add ticket

        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDTO addTicket)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.AddUserTicket(addTicket, User.GetUserId());
                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "پاسخ شما بزودی ارسال خواهد شد";
                        return RedirectToAction("Index");
                }
            }
            return View();
        }

        #endregion

        #region ticket detail

        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetail(long ticketId)
        {
            var ticket = await _contactService.GetTicketForShow(ticketId, User.GetUserId());
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        #endregion

        #region answer ticket

        [HttpPost("answer-ticket")]
        public async Task<IActionResult> AnswerTicket(AnswerTicketDTO answerTicket)
        {
            if (string.IsNullOrEmpty(answerTicket.Text))
            {
                TempData[ErrorMessage] = "لطفا پیام خود را وارد کنید!";
                RedirectToAction("TicketDetail", "Ticket", new { area = "User", ticketId = answerTicket.Id });
            }
            if (ModelState.IsValid)
            {
                var res = await _contactService.AnswerTicket(answerTicket, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی!";
                        TempData[WarningMessage] = "در صورت تکرار این مورد دسترسی شما بصورت کلی از سیستم قطع خواهد شد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد!";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "پیام شما با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("TicketDetail", "Ticket", new { area = "User", ticketId = answerTicket.Id });
        }

        #endregion
    }
}
