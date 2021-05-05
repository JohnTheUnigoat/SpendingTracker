using BL.Model.User;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Models.Auth.Response;
using SpendingTracker.Services;
using System;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("google-sign-in")]
        public async Task<ActionResult<AuthResponse>> SignInWithGoogle(string idToken)
        {
            var res = await _authService.GoogleSignIn(idToken);

            if (res == null)
            {
                return new StatusCodeResult(403);
            }

            return Ok(res);
        }

        [HttpGet("~/api/user")]
        public async Task<UserDomain> GetUser()
        {
            throw new NotImplementedException();
            //return await _userService.GetUserIdByGoogleId("asdf");
        }
    }
}
