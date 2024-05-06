using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.DTOs.Order;
using Vendor.DataLayer.Entities.ProductOrder;

namespace Vendor.Application.Services.interfaces
{
    public interface IOrderService: IAsyncDisposable
    {
        #region order

        Task<long> AddOrderForUser(long userId);
        Task<Order> GetLatestOpenOrderOfUser(long userId);
        Task<UserOpenOrderDTO> GetOpenUserDetail(long userId);

        #endregion

        #region order detail

        Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order);

        #endregion
    }
}
