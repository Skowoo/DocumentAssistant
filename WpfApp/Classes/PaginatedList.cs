using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp.Classes
{
    internal class PaginatedList<T> : List<T>
    {
        public int TotalPages { get; private set; }

        public int CurrentPage { get; private set; }

        public PaginatedList(List<T> items, int count, int requestedPage, int totalPagesCount)
        {
            CurrentPage = requestedPage;
            TotalPages = (int)Math.Ceiling(count / (double)totalPagesCount);
            this.AddRange(items);
        }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
