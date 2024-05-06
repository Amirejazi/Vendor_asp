using Microsoft.EntityFrameworkCore;
using Vendor.Application.Services.interfaces;
using Vendor.DataLayer.DTOs.Order;
using Vendor.DataLayer.Entities.ProductOrder;
using Vendor.DataLayer.Repository;

namespace Vendor.Application.Services.implementions
{
    public class OrderService: IOrderService
    {
        #region Ctor

        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;

        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _orderRepository.DisposeAsync();
            await _orderDetailRepository.DisposeAsync();
        }

        #endregion

        #region order

        public async Task<long> AddOrderForUser(long userId)
        {
            var order = new Order()
            {
                UserId = userId,
            };

            await _orderRepository.AddEntity(order);
            await _orderRepository.SaveChanges();
            return order.Id;
        }

        public async Task<Order> GetLatestOpenOrderOfUser(long userId)
        {
            if (!await _orderRepository.GetQuery().AnyAsync(o => o.UserId == userId && !o.IsPaied))
                await AddOrderForUser(userId);

            var userOpenOrder = await _orderRepository.GetQuery()
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.ProductColor)
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .SingleOrDefaultAsync(o => o.UserId == userId && !o.IsPaied);

            return userOpenOrder;
        }

        public async Task<UserOpenOrderDTO> GetOpenUserDetail(long userId)
        {
	        var openOrder = await GetLatestOpenOrderOfUser(userId);
	        return new UserOpenOrderDTO()
	        {
		        UserId = userId,
		        Description = openOrder.Description,
		        OrderDetails = openOrder.OrderDetails.Select(s => new UserOpenOrderDetailDTO()
		        {
                    ProductId = s.Product.Id,
                    ProductTitle = s.Product.Title,
                    ProductImageName = s.Product.ImageName,
                    ProductPrice = s.Product.Price,
                    ProductColorId = s.ProductColorId,
                    ColorName = s.ProductColor?.ColorName,
                    ProductColorPrice = s.ProductColor != null ? s.ProductColor.Price : 0,
                    Count = s.Count,

				}).ToList()
	        };
        }

        #endregion

        #region order detail


        public async Task AddProductToOpenOrder(long userId, AddProductToOrderDTO order)
        {
            var openOrder = await GetLatestOpenOrderOfUser(userId);

            var openOrderDetail =  await _orderDetailRepository.GetQuery()
                .SingleOrDefaultAsync(od => od.OrderId==openOrder.Id &&
                od.ProductId == order.ProductId &&
                od.ProductColorId == order.ProductColorId);

            if (openOrderDetail == null)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = openOrder.Id,
                    ProductId = order.ProductId,
                    ProductColorId = order.ProductColorId,
                    Count = order.Count
                };
                await _orderDetailRepository.AddEntity(orderDetail);
                await _orderDetailRepository.SaveChanges();
            }
            else
            {
                openOrderDetail.Count += 1;
                await _orderDetailRepository.SaveChanges();
            }
        }

        #endregion

    }
}
