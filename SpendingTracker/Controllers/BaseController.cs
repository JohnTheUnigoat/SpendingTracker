using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SpendingTracker.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public int UserId {
            get
            {
                var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (idValue == null)
                {
                    throw new HttpStatusException(401);
                }

                return int.Parse(idValue);
            }
        }
    }
}
