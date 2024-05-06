using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor.DataLayer.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Biuld(int pageId, int allEntityCount, int take, int howManyShowAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntityCount /(double) take));
            return new BasePaging
            {
                PageId = pageId,
                AllEntityCount = allEntityCount,
                TakeEntity = take,
                SkipEntity = (pageId - 1) * take,
                StartPage = pageId - howManyShowAfterAndBefore <= 0 ? 1 : pageId - howManyShowAfterAndBefore ,
                EndPage = pageId + howManyShowAfterAndBefore > pageCount ? pageCount : pageId + howManyShowAfterAndBefore,
                HowManyShowAfterAndBefore = howManyShowAfterAndBefore,
                PageCount = pageCount
            };
        }
    }
}
