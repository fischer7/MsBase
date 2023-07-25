namespace Fischer.Core.Application.Abstractions;

public abstract record PaginatedQueryInput
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}