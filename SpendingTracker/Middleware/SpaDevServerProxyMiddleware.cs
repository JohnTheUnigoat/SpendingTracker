using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpendingTracker.Middleware
{
    public class SpaDevServerProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _devServerUrl;

        private readonly HttpClient _httpClient = new HttpClient();

        public SpaDevServerProxyMiddleware(RequestDelegate next, string devServerUrl)
        {
            _next = next;
            _devServerUrl = devServerUrl;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            string targetUrl = _devServerUrl + context.Request.Path;

            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(targetUrl)
            });

            await response.Content.CopyToAsync(context.Response.Body);
        }
    }
}
