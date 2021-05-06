using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Models.Response;

namespace SpendingTracker.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public Test GetTest()
        {
            return new Test
            {
                Number = 42,
                Text = "That is the answer"
            };
        }
    }
}
