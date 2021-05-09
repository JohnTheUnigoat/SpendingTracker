using BL.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpendingTracker.Middleware
{
    public class WalletAuthorizationMiddleware
    {
        const string walletIdKey = "walletId";

        private readonly RequestDelegate _next;

        public WalletAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IWalletService walletService)
        {
            if (context.Request.Path.StartsWithSegments("/api/wallets") == false)
            {
                await _next(context);
                return;
            }

            if (context.Request.RouteValues.ContainsKey(walletIdKey) == false)
            {
                await _next(context);
                return;
            }

            int walletId = int.Parse(context.Request.RouteValues[walletIdKey] as string);

            int userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (await walletService.IsUserAuthorizedForWalletAsync(walletId, userId) == false)
            {
                context.Response.StatusCode = 401;
                return;
            }

            await _next(context);
        }
    }
}
