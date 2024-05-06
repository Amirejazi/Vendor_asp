using Microsoft.AspNetCore.Mvc;
using Vendor.Application.Services.interfaces;

namespace Vendor.Web.Areas.Seller.ViewComponents
{
    public class SellerSidebarViewComponent: Microsoft.AspNetCore.Mvc.ViewComponent
    {
        #region Ctor


        #endregion


        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return View("SellerSidebar");
        }
    }
}
