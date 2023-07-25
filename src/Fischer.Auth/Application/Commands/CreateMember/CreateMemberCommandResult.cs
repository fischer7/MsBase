using Fischer.Core.Domain.Shared;

namespace Fischer.Auth.Application.Commands.CreateMember;
internal sealed record CreateMemberCommandResult(bool IsSuccess, Error Error);
