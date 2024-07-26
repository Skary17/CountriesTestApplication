using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesTestApplication.Infraestructure.Utilities
{
    public static class PaginationExtensions
    {
        public static PagedResult<T> GetPaged<T>(this IEnumerable<T> query, int page, int pageSize)
        {
            var totalItems = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<T>(items, page, pageSize, totalItems);
        }
    }
}
