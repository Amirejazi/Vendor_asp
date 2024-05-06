using Vendor.DataLayer.DTOs.Common;
using Vendor.DataLayer.DTOs.Seller;
using Vendor.DataLayer.Entities.Store;

namespace Vendor.Application.Services.implementions
{
    public interface ISellerService: IAsyncDisposable
    {
        #region seller

        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO requestSeller, long userId);
        Task<FilterSellerDTO> GetFilterSellers(FilterSellerDTO  filterSeller);
        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId);
        Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO  editRequestSeller, long currentUserId);
        Task<bool> AcceptSellerRequest(long requestId);
        Task<bool> RejectSellerRequest(RejectItemDTO rejectItem);
        Task<Seller> GetLastActiveSellerByUserId(long userId);
        Task<bool> HasUserAcitveSellerPanel(long userId);

        #endregion
    }
}