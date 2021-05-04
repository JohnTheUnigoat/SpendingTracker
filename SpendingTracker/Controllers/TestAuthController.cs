using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpendingTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestAuthController : ControllerBase
    {
        [HttpGet]
        public string TestAuth()
        {
            return "you are logged in!";
        }
    }
}
