using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blecommerce.Shared
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                PageSize = pageSize,
                TotalCount = count
            };
            AddRange(items);
        }

        public MetaData MetaData { get; set; }

        public static async Task<PagedList<T>> ToBagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await Task.Run(() => source.Count()); 

            var items = await Task.Run(()=> source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());  
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

    }

}
