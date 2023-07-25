using Fischer.Auth.Application.Queries.GetUsers;
using Fischer.Auth.Application.Queries.Login;
using Fischer.Core.Application.Filters;
using Fischer.Core.Controllers;
using Fischer.Core.Domain.Enumerators;
using Fischer.Core.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;


namespace Fischer.Auth.Controllers;

[ApiController]
public class AuthController : ApiController
{
    [HttpPost]
    [Route("/api/v1/Auth/login")]
    [AllowAnonymous]
    [SwaggerResponse((int)HttpStatusCode.OK, "", typeof(Result<LoginUserQueryResult>))]
    public async Task<IActionResult> Login(LoginUserQueryInput request, CancellationToken cancellationToken)
    {
        return CustomResponse(await Mediator.Send(request, cancellationToken));
    }

    [HttpGet]
    [Route("/api/v1/Auth/getUsers")]
    [AuthorizationFilter(RolePermissionEnum.Administrator, PermissionActionEnum.Special)]
    [Authorize()]
    public async Task<IActionResult> GetUser([FromQuery]GetUsersQueryInput request, CancellationToken cancellationToken)
    {
        return CustomResponse(await Mediator.Send(request, cancellationToken));
    }
}