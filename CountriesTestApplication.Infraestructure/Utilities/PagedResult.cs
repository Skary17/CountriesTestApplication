using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesTestApplication.Infraestructure.Utilities
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public List<T> Items { get; set; }

        public PagedResult(IEnumerable<T> items, int page, int pageSize, int totalItems)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items.ToList();
        }
    }
}
