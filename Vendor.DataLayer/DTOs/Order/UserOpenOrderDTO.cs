namespace Vendor.DataLayer.DTOs.Order
{
	public class UserOpenOrderDTO
	{
		public long UserId { get; set; }

		public string Description { get; set; }

		public List<UserOpenOrderDetailDTO> OrderDetails { get; set; }

		public int GetTotalPrice()
		{
			return OrderDetails.Sum(s => (s.ProductPrice + s.ProductColorPrice) * s.Count);
		}
	}
}
