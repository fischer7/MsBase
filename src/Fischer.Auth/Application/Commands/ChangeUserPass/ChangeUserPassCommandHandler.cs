using Fischer.Auth.Domain.Entities;
using Fischer.Auth.Persistence.Repositories.Interfaces;
using Fischer.Core.Application.Abstractions;
using Fischer.Core.Constants;
using Fischer.Core.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Fischer.Auth.Application.Commands.ChangeUserPass;
internal sealed class ChangeUserPassCommandHandler : ICommandHandler<ChangeUserPassCommandInput, ChangeUserPassCommandResult>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserRepository _userRepository;

    public ChangeUserPassCommandHandler(
        UserManager<AppUser> userManager,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<Result<ChangeUserPassCommandResult>> Handle(ChangeUserPassCommandInput request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue
            || request.NewPass is null 
            || request.NewPassConfirmation is null)
        {
            return new ChangeUserPassCommandResult(false, ErrorConstants.UserPassNullValue);
        }

        var user = await _userManager.FindByIdAsync(request.UserId.ToString()!);

        if (user is null)
        {
            return new ChangeUserPassCommandResult(false, ErrorConstants.None);
        }


        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPass);

        if (result.Succeeded)
        {
            if (user.ShouldAlterPass)
            {
                user.SetAlteredPassword();
            }

            await _userRepository.UnitOfWork.CommitAsync();

            return new ChangeUserPassCommandResult(false, ErrorConstants.None); 
        }

        var erros = result.Errors.Select(e => e.Description)?.ToList() ?? new List<string>();
        return new ChangeUserPassCommandResult(erros.Any(), erros.Select(x => new Error("Fail", x))?.FirstOrDefault() ?? ErrorConstants.None);
    }
}