using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Site;

namespace Vendor.Application.Services.interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        #region site setting

        Task<SiteSetting> GetDefaultSiteSetting();

        #endregion

        #region slider

        Task<List<Slider>> GetAllActiveSliders();

        #endregion

        #region banner

        Task<List<SiteBanner>> GetAllActiveBannersByPalcement(List<BannerPlacement> palacements);

        #endregion
    }
}
