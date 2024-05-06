using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.Application.Utils
{
	public static class PathExtentions
	{
		#region default images

		public static string DefaultAvatarOrigin = "/img/defaults/default.png";

		#endregion

		#region uplader

		public static string UploadImage = "/img/ck-uploads/";
		public static string UploadImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/ck-uploads/");

		#endregion

		#region product

		public static string ProductImage = "/Content/Images/product/origin/";
		public static string ProductImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/product/origin/");

		public static string ProductThumbnailImage = "/Content/Images/product/thumb/";
		public static string ProductThumbnailImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/product/thumb/");

        #endregion

        #region product gallery

        public static string ProductGalleryImage = "/Content/Images/product-gallery/origin/";
        public static string ProductGalleryImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/product-gallery/origin/");

        public static string ProductGalleryThumbnailImage = "/Content/Images/product-gallery/thumb/";
        public static string ProductGalleryThumbnailImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/product-gallery/thumb/");

        #endregion

        #region user

        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/origin/";
		public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

		public static string UserAvatarThumb = "/Content/Images/UserAvatar/thumb/";
		public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/thumb/");

		#endregion

		#region slider

		public static string SliderOrigin = "/img/slider/";

		#endregion

		#region banner

		public static string BannerOrigin = "/img/bg/";

		#endregion
	}
}
