using Fischer.Core.Application.Abstractions;
using Fischer.Core.Application.Attributes;

namespace Fischer.Auth.Application.Commands.ChangeUserPass;
public sealed record ChangeUserPassCommandInput : ICommand<ChangeUserPassCommandResult>
{
    public Guid? UserId { get; set; }

    [SensitiveData]
    public string? NewPass { get; set; }

    [SensitiveData]
    public string? NewPassConfirmation { get; set; }

}
