using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Models.Auth.Response;
using SpendingTracker.Services;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("google-sign-in")]
        public async Task<ActionResult<AuthResponse>> SignInWithGoogle([FromBody] string idToken)
        {
            var res = await _authService.GoogleSignIn(idToken);

            return Ok(res);
        }
    }
}
