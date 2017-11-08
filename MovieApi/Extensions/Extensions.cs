namespace ExtensionMethods{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Paginator;
    public static class Extensions{
        public static Page<T> GetPage<T>(this DbSet<T> list, int page_index, int page_size, Func<T,object> order_by_selector)
        where T : class
        {
            var result = list.OrderBy(order_by_selector)
                .Skip(page_index * page_size)
                .Take(page_size)
                .ToArray();

            if(result == null || result.Length == 0)
            {
                return null;
            }

            var totalItems = list.Count();

            var totalPages = totalItems/page_size;
            if(totalItems < page_size) totalPages = 1;

            return new Page<T>(){Index = page_index, Items = result, TotalPages = totalPages};
        }
    }
}