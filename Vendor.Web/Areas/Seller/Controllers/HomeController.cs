using Microsoft.AspNetCore.Mvc;

namespace Vendor.Web.Areas.Seller.Controllers
{
    public class HomeController : SellerBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
