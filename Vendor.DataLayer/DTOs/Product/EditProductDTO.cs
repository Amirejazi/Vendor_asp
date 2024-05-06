using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.DataLayer.DTOs.Product
{
	public class EditProductDTO: CreateProductDTO
	{
		public long Id { get; set; }

        public string ImageName { get; set; }
	}

    public enum EditProductResult{
		Success,
		NotFound,
		NotForUser,
		Error
    }
}
