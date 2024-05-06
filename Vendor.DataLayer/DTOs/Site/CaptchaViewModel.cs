using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.DataLayer.DTOs.Site
{
    public class CaptchaViewModel
    {
        [Required]
        public string Captcha { get; set; }
    }
}
