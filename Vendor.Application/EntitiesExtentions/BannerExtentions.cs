using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Application.Utils;
using Vendor.DataLayer.Entities.Site;

namespace Vendor.Application.EntitiesExtentions
{
    public static class BannerExtentions
    {
        public static string GetBannerImageAddress(this SiteBanner banner)
        {
            return PathExtentions.BannerOrigin + banner.ImageName;
        }
    }
}
