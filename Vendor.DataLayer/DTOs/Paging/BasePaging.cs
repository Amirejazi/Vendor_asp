
namespace Vendor.DataLayer.DTOs.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            PageId = 1;
            TakeEntity = 10;
            HowManyShowAfterAndBefore = 3;
        }
        public int PageId { get; set; }

        public int PageCount { get; set; }

        public int AllEntityCount { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public int TakeEntity { get; set; }

        public int SkipEntity { get; set; }

        public int HowManyShowAfterAndBefore { get; set; }

        public int GetLastPage()
        {
            return (int)Math.Ceiling(AllEntityCount / (double)TakeEntity);
        } 

        public string GetCurrentPagingStatus()
        {
            var startItem = 1;
            var endItem = AllEntityCount;

            if (EndPage > 1)
            { 
                startItem = (PageId - 1) * TakeEntity;
                endItem = (PageId * TakeEntity > AllEntityCount)? AllEntityCount: PageId * TakeEntity;
            }
            return $"نمایش {startItem}-{endItem} از {AllEntityCount}";
        }

        public BasePaging GetCurrentPaging()
        {
            return this;
        }
    }
}
