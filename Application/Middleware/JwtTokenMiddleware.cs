using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Application.Services;

namespace Application.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly TokenManager _tokenManager; // Inject the TokenManager

        public JwtTokenMiddleware(RequestDelegate next, IConfiguration configuration, TokenManager tokenManager)
        {
            _next = next;
            _configuration = configuration;
            _tokenManager = tokenManager; // Injected TokenManager
        }

        public async Task Invoke(HttpContext context)
        {
            // Extract the token from the request headers
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // If no token is present, return an unauthorized response
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            if (_tokenManager.ValidateToken(token))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Add("WWW-Authenticate", "Bearer");
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }
    }
}
