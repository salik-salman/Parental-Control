using Parental_Control.Data;
using Microsoft.AspNetCore.Mvc.Controllers;
using Parental_Control.Controllers;

namespace Parental_Control.Middlwares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, Parental_ControlContext _context)
        {
            AuthController user = new AuthController(_context);
            Boolean isloggedin = false;
            string userId = null;
            string userName = null;
            try
            {
                userId = httpContext.Session.GetString("userId");
                userName = httpContext.Session.GetString("username");
            }
            catch (NullReferenceException e)
            { }
            if (userId != null && userName != null)
            {
                var User = _context.Users
                .Where(users => users.id == Int32.Parse(userId))
                .Where(users => users.username == userName).FirstOrDefault();
                if (User != null && User.logged_in == 1)
                {
                    isloggedin =  true;
                }
                else
                {
                    if (User != null) {
                        user.logout();
                    }
                    isloggedin =  false;
                }
            }
            else
            {
                isloggedin =  false;
            }

            var controllerActionDescriptor = httpContext
            .GetEndpoint()
            .Metadata
            .GetMetadata<ControllerActionDescriptor>();
            string controllerName = controllerActionDescriptor.ControllerName;
            var actionName = controllerActionDescriptor.ActionName;
            string Value = "";
            Login_Exclude Excludes = new Login_Exclude();
            IDictionary<string, string> LoginExcludes = Excludes.Login_Excludes();
            string referer = httpContext.Request.Headers["Referer"].ToString();
            if (isloggedin == false && controllerName != "Auth")
            {
                if (LoginExcludes.TryGetValue(controllerName, out Value))
                {
                    if (Value == actionName || controllerName == Value)
                    {
                        await _next(httpContext);
                    }
                    else
                    {
                        httpContext.Items["Status"] = "Please Login To Continue.";
                        httpContext.Response.Redirect("/auth");
                        await _next(httpContext);
                    }
                }
                else
                {
                    httpContext.Items["Status"] = "Please Login To Continue";
                    httpContext.Response.Redirect("/auth");
                    await _next(httpContext);
                }
            }
            else
            {

                if (referer != "")
                {
                    httpContext.Items["Status"] = "Please Login To Continue.";
                }
                await _next(httpContext);
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}
