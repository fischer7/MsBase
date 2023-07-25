using Fischer.Core.Application.Abstractions;

namespace Fischer.Auth.Application.Commands.CreateMember;

public sealed record CreateMemberCommandInput(string Email,string FirstName,string LastName) 
        : ICommand<CreateMemberCommandResult>;
