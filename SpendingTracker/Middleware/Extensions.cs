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
    }
}
