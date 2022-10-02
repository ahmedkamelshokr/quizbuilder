using Application.Users.Commands;
using Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    public class UserController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public UserController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserCommand payload)
        {
            var authUser = await _mediator.Send(payload);

            return Ok(authUser);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateUserQuery payload)
        {
            var authUser = await _mediator.Send(payload);

            return Ok(authUser);
        }

    }
}
