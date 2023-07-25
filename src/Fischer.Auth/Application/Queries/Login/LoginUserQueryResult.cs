using Fischer.Core.Domain.Shared;

namespace Fischer.Auth.Application.Queries.Login;

internal sealed record LoginUserQueryResult(bool isSuccess, string? token, Error error);