using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Vendor.Application.Extentions
{
    public static class CommenExtetion
    {
        public static string GetEnumName(this Enum myEnum)
        {
            var enumDisplayName = myEnum.GetType().GetMember(myEnum.ToString()).FirstOrDefault();
            if (enumDisplayName != null)
            {
                return enumDisplayName.GetCustomAttribute<DisplayAttribute>()?.GetName();
            }

            return "";
        }
    }
}
