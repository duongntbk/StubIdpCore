using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace StubIdpCore.Middleware
{
    // Pipeline to access BaseUrl of WebApp.
    // Based on https://stackoverflow.com/a/47051481/4510614
    public class MyHttpContext
    {
        private static IHttpContextAccessor m_httpContextAccessor;

        private static HttpContext Current => m_httpContextAccessor.HttpContext;

        public static string AppBaseUrl =>
            $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

        public static Uri AppBaseUri => new Uri(AppBaseUrl);

        internal static void Configure(IHttpContextAccessor contextAccessor) =>
            m_httpContextAccessor = contextAccessor;
    }

    public static class HttpContextExtensions
    {
        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            MyHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
