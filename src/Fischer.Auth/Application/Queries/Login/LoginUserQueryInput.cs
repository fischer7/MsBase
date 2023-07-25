using Fischer.Core.Application.Abstractions;
using Fischer.Core.Application.Attributes;

namespace Fischer.Auth.Application.Queries.Login;

public sealed record LoginUserQueryInput : IQuery<LoginUserQueryResult>
{
    public string UserName { get; set; } = default!;

    [SensitiveData]
    public string Password { get; set; } = default!;

}