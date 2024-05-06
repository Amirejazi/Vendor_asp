using MarketPlace.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vendor.Application.Utils;

namespace Vendor.Web.Controllers
{
    public class UploaderController : SiteBaseController
    {
        [HttpPost("ckupload-image")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "لطفا یک تصویر انتخاب کنید!";
                var notImage = JsonConvert.DeserializeObject("{'uploaded': 0, 'error':{'message': \"" + notImageMessage + " \"} }");
                return Json(notImage);
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            upload.AddImageToServer(fileName, PathExtentions.UploadImageServer, null, null);

            var url = $"{PathExtentions.UploadImage}{fileName}";

            return Json(new { uploaded = true, url });
        }
    }
}
