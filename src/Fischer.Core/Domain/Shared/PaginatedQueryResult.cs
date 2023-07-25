using Microsoft.EntityFrameworkCore;

namespace Fischer.Core.Domain.Shared;

public sealed record PaginatedQueryResult<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedQueryResult(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public static async Task<PaginatedQueryResult<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedQueryResult<T>(items, count, pageNumber, pageSize);
    }
}
