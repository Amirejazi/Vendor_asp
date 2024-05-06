using Microsoft.AspNetCore.Mvc;

namespace Vendor.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        #region Ctor



        #endregion

        #region dashboard

        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        #endregion
    }
}
