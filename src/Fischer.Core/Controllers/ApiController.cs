using Fischer.Core.Constants;
using Fischer.Core.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleFailure(Result? result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result?.Error ?? ErrorConstants.NullValue))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        List<Error>? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };

    protected IActionResult CustomResponse(Result? input)
    {
        if (input is null)
            return HandleFailure(input);
        else if (input.IsSuccess)
            return Ok(input);
        else
            return HandleFailure(input);

    }
}
