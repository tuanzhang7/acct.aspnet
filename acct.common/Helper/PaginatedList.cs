using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acct.common.Helper
{
    public class PaginatedList<T> : List<T>, IPagination
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int StartIndex { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public string QueryString { get; set; }
        public PaginatedList(IList<T> source, int pageIndex, int pageSize, int totalCounter,bool SkipSource)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCounter;// source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (SkipSource)
            {
                this.AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize));//
            }
            else
            {
                this.AddRange(source);
            }
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
