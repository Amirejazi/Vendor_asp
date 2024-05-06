using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vendor.Application.Services.implementions;

namespace Vendor.Web.PresentationExtentions
{
    public class CheckSellerAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        private ISellerService _sellerService;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _sellerService = (ISellerService) context.HttpContext.RequestServices.GetService(typeof(ISellerService));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetUserId();

                if (!_sellerService.HasUserAcitveSellerPanel(userId).Result)
                {
                    context.Result = new RedirectResult("/user");
                }
            }
        }
    }
}
