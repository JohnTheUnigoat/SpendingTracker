using Microsoft.AspNetCore.Builder;

namespace SpendingTracker.Middleware
{
    public static class Extensions
    {
        public static IApplicationBuilder UseSpaDevServerProxy(this IApplicationBuilder app, string devServerUrl)
        {
            app.UseMiddleware<SpaDevServerProxyMiddleware>(devServerUrl);

            return app;
        }

        public static IApplicationBuilder UseWalletAuthorization(this IApplicationBuilder app)
        {
            app.UseMiddleware<WalletAuthorizationMiddleware>();

            return app;
        }
    }
}
