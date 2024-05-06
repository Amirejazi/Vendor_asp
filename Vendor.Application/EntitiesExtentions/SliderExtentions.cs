using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.Application.Utils;
using Vendor.DataLayer.Entities.Site;

namespace Vendor.Application.EntitiesExtentions
{
    public static class SliderExtentions
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return PathExtentions.SliderOrigin + slider.ImageName;
        }
    }
}
