﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.Application.Services.interfaces
{
    public interface IPasswordHelper
    {
        string EncodePasswordMd5(string pass);
    }
}
