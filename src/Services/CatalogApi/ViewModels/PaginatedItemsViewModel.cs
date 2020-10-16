using System.Collections.Generic;
namespace CatalogApi.ViewModels
{
    public class PaginatedItemsViewModel<T> where T:class
    {
        public PaginatedItemsViewModel(int pageSize,int pageIndex,long count,IEnumerable<T> data)
        {
            PageSize=pageSize;
            PageIndex=pageIndex;
            Count=count;
            Data=data;
        }
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public long Count { get; set; }

        public IEnumerable<T> Data{ get; set; }
    }
}