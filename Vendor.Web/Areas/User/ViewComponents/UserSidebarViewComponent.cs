using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.interfaces;

namespace Vendor.Web.Areas.User.ViewComponents
{
    public class UserSidebarViewComponent: Microsoft.AspNetCore.Mvc.ViewComponent
    {
        #region Ctor


        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return View("UserSidebar");
        }
    }
}
