using Fischer.Core.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fischer.Core.Extensions;

public static class PaginationExtensions
{
    public static Task<PaginatedQueryResult<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedQueryResult<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
}
