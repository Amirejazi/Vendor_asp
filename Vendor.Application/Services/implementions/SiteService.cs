using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Entities.Site;
using Vendor.DataLayer.Repository;

namespace Vendor.Application.Services.implementions
{
    public class SiteService : ISiteService
    {
        #region Ctor

        private readonly IGenericRepository<SiteSetting> _siteRepository;
        private readonly IGenericRepository<Slider> _sliderRepository;
        private readonly IGenericRepository<SiteBanner> _bannerRepository;

        public SiteService(IGenericRepository<SiteSetting> siteRepository, IGenericRepository<Slider> sliderRepository, IGenericRepository<SiteBanner> bannerRepository)
        {
            _siteRepository = siteRepository;
            _sliderRepository = sliderRepository;
            _bannerRepository = bannerRepository;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            if (_siteRepository != null) await _siteRepository.DisposeAsync();
            if (_sliderRepository != null) await _sliderRepository.DisposeAsync();
            if (_bannerRepository != null) await _bannerRepository.DisposeAsync();
        }

        #endregion

        #region site setting

        public async Task<SiteSetting> GetDefaultSiteSetting()
        {
            return await _siteRepository.GetQuery().SingleOrDefaultAsync(s => s.IsDefault && !s.IsDelete);
        }

        #endregion

        #region slider

        public async Task<List<Slider>> GetAllActiveSliders()
        {
            return await _sliderRepository.GetQuery().Where(s=>s.IsActive && !s.IsDelete).ToListAsync();
        }

        #endregion

        #region banner

        public async Task<List<SiteBanner>> GetAllActiveBannersByPalcement(List<BannerPlacement> palacements)
        {
            return await _bannerRepository.GetQuery().Where(s=>palacements.Contains(s.BannerPlacement)).ToListAsync();
        }

        #endregion
    }
}
