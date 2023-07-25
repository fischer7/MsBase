using Fischer.Core.Application.Abstractions;
using Fischer.Core.Domain.Shared;

namespace Fischer.Auth.Application.Queries.GetUsers;

public sealed record GetUsersQueryInput : PaginatedQueryInput, IQuery<PaginatedQueryResult<GetUsersQueryResult>>;