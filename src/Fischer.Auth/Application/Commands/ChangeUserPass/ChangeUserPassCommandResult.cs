using Fischer.Core.Domain.Shared;

namespace Fischer.Auth.Application.Commands.ChangeUserPass;

internal sealed record ChangeUserPassCommandResult(bool isSuccess, Error error);