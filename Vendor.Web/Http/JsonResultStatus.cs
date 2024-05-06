using Microsoft.AspNetCore.Mvc;

namespace Vendor.Web.Http
{
    public static class JsonResultStatus
    {
        public static JsonResult SendStatus(JsonStatusType type, string message, object data)
        {
            return new JsonResult(new{status = type.ToString(), message=message, data=data});
        }
    }

    public enum JsonStatusType
    {
        Success,
        Danger,
        Warning,
        Info
    }
}
