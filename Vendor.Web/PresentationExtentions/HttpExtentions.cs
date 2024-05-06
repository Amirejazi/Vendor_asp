namespace Vendor.Web.PresentationExtentions
{
    public static class HttpExtentions
    {
        public static string GetUserIp(this HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
