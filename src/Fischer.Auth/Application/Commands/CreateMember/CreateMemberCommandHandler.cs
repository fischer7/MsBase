using Fischer.Core.Application.Abstractions;
using Fischer.Core.Constants;
using Fischer.Core.Domain.Shared;
using Fischer.Core.Infraestructure.Queues;
using Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;

namespace Fischer.Auth.Application.Commands.CreateMember;
internal sealed class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommandInput, CreateMemberCommandResult>
{
    private readonly IEmailBus _emailBus;

    public CreateMemberCommandHandler(IEmailBus emailBus)
        => _emailBus = emailBus;
    public async Task<Result<CreateMemberCommandResult>> Handle(CreateMemberCommandInput request, CancellationToken cancellationToken)
    {
        //return Result.Failure<CreateMemberCommandResult>(Error.NullValue);
        await Task.Delay(0, cancellationToken);
        var u = new UserSuccessfullyRegisteredEmailInput(request.FirstName, request.LastName);
        await _emailBus.Publish(u);

        return new CreateMemberCommandResult(false, ErrorConstants.None);
    }
}
