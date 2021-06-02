using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Auth.Response;
using SpendingTracker.Models.User.Response;
using SpendingTracker.Services;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("google-sign-in")]
        public async Task<ActionResult<AuthResponse>> SignInWithGoogle([FromBody] string idToken)
        {
            var res = await _authService.GoogleSignIn(idToken);

            return Ok(res);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<UserResponse> GetUser()
        {
            return (await _userService.GetUser(UserId)).ToResponse();
        }
    }
}
