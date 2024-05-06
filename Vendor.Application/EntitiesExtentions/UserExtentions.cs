using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor.DataLayer.Entities.Account;

namespace Vendor.Application.EntitiesExtentions
{
	public static class UserExtentions
	{
		public static string GetShowName(this User? user)
		{
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.FisrtName) && !string.IsNullOrEmpty(user.LastName))
                {
                    return $"{user.FisrtName} {user.LastName}";
                }

                return user.Mobile;
            }

            return "";
        }
	}
}
