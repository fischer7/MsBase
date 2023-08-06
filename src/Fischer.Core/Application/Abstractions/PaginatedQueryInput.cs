namespace Fischer.Core.Application.Abstractions;

public abstract record PaginatedQueryInput
{
    public int PageSize { get; set; } = 100;
    public int PageNumber { get; set; } = 0;
}