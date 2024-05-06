using Microsoft.AspNetCore.Mvc;

namespace Vendor.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseContoller
    {
        #region index

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
