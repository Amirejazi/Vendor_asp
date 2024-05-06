 using Microsoft.EntityFrameworkCore;
using Vendor.DataLayer.DTOs.Common;
using Vendor.DataLayer.DTOs.Paging;
using Vendor.DataLayer.DTOs.Seller;
using Vendor.DataLayer.Entities.Account;
using Vendor.DataLayer.Entities.Contacts;
using Vendor.DataLayer.Entities.Store;
using Vendor.DataLayer.Repository;

namespace Vendor.Application.Services.implementions
{
    public class SellerService : ISellerService
    {
        #region Ctor

        private readonly IGenericRepository<Seller> _sellerRepository;
        private readonly IGenericRepository<User> _userRepository;

        public SellerService(IGenericRepository<Seller> sellerRepository, IGenericRepository<User> userRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _sellerRepository.DisposeAsync();
            await _userRepository.DisposeAsync();
        }

        #endregion

        #region seller

        public async Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO requestSeller, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user.IsBlocked) return RequestSellerResult.HasNotPermission;

            var underProgressRequest = await _sellerRepository.GetQuery()
                .AnyAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);
            if (underProgressRequest) return RequestSellerResult.HasUnderProgressRequest;
            
            var newSeller = new Seller
            {
                UserId = userId,
                StoreName = requestSeller.StoreName,
                Phone = requestSeller.Phone,
                Address = requestSeller.Address,
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress
            };
            await _sellerRepository.AddEntity(newSeller);
            await _sellerRepository.SaveChanges();

            return RequestSellerResult.Success;
        }

        public async Task<FilterSellerDTO> GetFilterSellers(FilterSellerDTO filterSeller)
        {
            var query = _sellerRepository.GetQuery().Include(s => s.User).AsQueryable();

            #region state

            switch (filterSeller.State)
            {
                case FilterSellerState.All:
                    break;
                case FilterSellerState.Accepted:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
                    break;
                case FilterSellerState.Rejected:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Rejected);
                    break;
                case FilterSellerState.UnderProgress:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);
                    break;
            }

            #endregion

            #region filter

            if (filterSeller.UserId != null && filterSeller.UserId !=0)
            {
                query = query.Where(s => s.UserId == filterSeller.UserId);
            }

            if (!string.IsNullOrEmpty(filterSeller.StoreName))
            {
                query = query.Where(s => EF.Functions.Like(s.StoreName, $"%{filterSeller.StoreName}%"));
            }
            if (!string.IsNullOrEmpty(filterSeller.Phone))
            {
                query = query.Where(s => EF.Functions.Like(s.Phone, $"%{filterSeller.Phone}%"));
            }
            if (!string.IsNullOrEmpty(filterSeller.Mobile))
            {
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filterSeller.Mobile}%"));
            }
            if (!string.IsNullOrEmpty(filterSeller.Address))
            {
                query = query.Where(s => EF.Functions.Like(s.Address, $"%{filterSeller.Address}%"));
            }

            #endregion

            #region paging

            var entityCount = await query.CountAsync();
            var pager = Pager.Biuld(filterSeller.PageId, entityCount, filterSeller.TakeEntity, filterSeller.HowManyShowAfterAndBefore);
            var allEntity = await query.Paging(pager).ToListAsync();

            #endregion

            filterSeller.SetPaging(pager).SetSellers(allEntity);
            return filterSeller;
        }

        public async Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(id);
            if (seller == null || seller.UserId != currentUserId) return null;

            return new EditRequestSellerDTO
            {
                Id = seller.Id,
                StoreName = seller.StoreName,
                Phone = seller.Phone,
                Address = seller.Address,
            };
        }

        public async Task<EditRequestSellerResult> EditRequestSeller(EditRequestSellerDTO editRequestSeller, long currentUserId)
        {
            var requestSeller = await _sellerRepository.GetEntityById(editRequestSeller.Id);
            if (requestSeller == null || requestSeller.UserId != currentUserId) return EditRequestSellerResult.NotFound;

            requestSeller.StoreName = editRequestSeller.StoreName;
            requestSeller.Phone = editRequestSeller.Phone;
            requestSeller.Address = editRequestSeller.Address;
            requestSeller.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;

            _sellerRepository.EditEntity(requestSeller);
            await _sellerRepository.SaveChanges();
            return EditRequestSellerResult.Success;
        }

        public async Task<bool> AcceptSellerRequest(long requestId)
        {
            var sellerRequest = await _sellerRepository.GetEntityById(requestId);
            if (sellerRequest != null)
            {
                sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                sellerRequest.StoreAcceptanceDescription = "اطلاعات پنل فروشندگی شما تایید شده است";
                _sellerRepository.EditEntity(sellerRequest);
                await _sellerRepository.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerRequest(RejectItemDTO rejectItem)
        {
            var sellerRequest = await _sellerRepository.GetEntityById(rejectItem.Id);
            if (sellerRequest != null)
            {
                sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Rejected;
                sellerRequest.StoreAcceptanceDescription = rejectItem.RejectMessage;
                _sellerRepository.EditEntity(sellerRequest);
                await _sellerRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<Seller> GetLastActiveSellerByUserId(long userId)
        {
            var seller = await _sellerRepository.GetQuery()
                .OrderByDescending(s => s.CreateDate)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
            return seller;
        }

        public async Task<bool> HasUserAcitveSellerPanel(long userId)
        {
            return await _sellerRepository.GetQuery()
                .OrderByDescending(s => s.CreateDate)
                .AnyAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        #endregion
    }
}
