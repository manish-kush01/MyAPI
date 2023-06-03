using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
               string encodeUsernameAndPassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding= Encoding.UTF8;
                string usernameAndPassword = encoding.GetString(Convert.FromBase64String(encodeUsernameAndPassword));
                int index = usernameAndPassword.IndexOf(':');
                var username= usernameAndPassword.Substring(0, index);
                var password = usernameAndPassword.Substring (index + 1);
                if(username.Equals("abc") && password.Equals("123"))
                {
                    await _next(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
            }

            httpContext.Response.StatusCode = 401;
            return;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BasicAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
